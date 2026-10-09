using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BeanFileBrowser : MonoBehaviour
{
    [SerializeField] private GameObject UIRoot;
    [SerializeField] private GameObject SelectFileUI;
    [SerializeField] private GameObject SaveFileUI;
    
    [SerializeField] private GameObject QuickAccessItem;
    [SerializeField] private Transform QuickAccessRoot;
    [SerializeField] private GameObject FileItem;
    [SerializeField] private Transform FileRoot;

    [SerializeField] private Sprite FolderSprite;
    [SerializeField] private Sprite FileSprite;

    [SerializeField] private TMP_InputField PathInput;
    [SerializeField] private TMP_InputField FileNameInput;

    [SerializeField] private TMP_Text TitleText;

    private bool SaveMode;

    private string CurrentFileExt;
    private Action<string> CurrentSelectAction;
    private Action CurrentCancelAction;

    public static BeanFileBrowser instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        List<BeanFileBrowserQuickAccessItem> quickAccessItems = new();

        DriveInfo[] drives = DriveInfo.GetDrives();
        
#if UNITY_STANDALONE_WIN
        string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string PicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        string VideosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        string MusicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

        IntPtr pPath;
        string DownloadsPath;
        if (WindowsAPI.SHGetKnownFolderPath(KnownFolder.Downloads, 0, IntPtr.Zero, out pPath) == 0)
        {
            DownloadsPath = Marshal.PtrToStringUni(pPath);
            Marshal.FreeCoTaskMem(pPath);
        }
        else
        {
            DownloadsPath = string.Empty;
        }
        
        quickAccessItems.Add(new(UserPath, UserPath));
        quickAccessItems.Add(new(DesktopPath, DesktopPath));
        quickAccessItems.Add(new(DownloadsPath, DownloadsPath));
        quickAccessItems.Add(new(DocumentsPath, DocumentsPath));
        quickAccessItems.Add(new(PicturesPath, PicturesPath));
        quickAccessItems.Add(new(VideosPath, VideosPath));
        quickAccessItems.Add(new(MusicPath, MusicPath));
#endif

        foreach (var drive in drives)
        {
            quickAccessItems.Add(new(drive.Name, drive.RootDirectory.FullName));
        }

        foreach (var quickAccessItem in quickAccessItems)
        {
            GameObject go = Instantiate(QuickAccessItem, QuickAccessRoot);
            go.GetComponentInChildren<TMP_Text>().text = quickAccessItem.Name;
            go.GetComponent<Button>().onClick.AddListener(() => GotoDirectory(quickAccessItem.Path));
        }
    }

    private void GotoDirectory(string path)
    {
        string[] dirs;
        string[] files;
        
        try
        {
            dirs = Directory.GetDirectories(path);
            files = Directory.GetFiles(path);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            
            Notification.instance.ShowNotification("Failed to load directory\n" + ex.Message, "Error");
            
            return;
        }
        
        foreach (Transform oldFile in FileRoot)
        {
            Destroy(oldFile.gameObject);
        }

        PathInput.text = path;

        GameObject backGo = Instantiate(FileItem, FileRoot);
        backGo.GetComponentInChildren<TMP_Text>().text = "..";
        backGo.transform.GetChild(0).GetComponentInChildren<Image>().sprite = FolderSprite;
        backGo.GetComponent<Button>().onClick.AddListener(() => GotoDirectory(Directory.GetParent(path).FullName));

        foreach (var dir in dirs)
        {
            GameObject go = Instantiate(FileItem, FileRoot);
            go.GetComponentInChildren<TMP_Text>().text = Path.GetFileName(dir);
            go.transform.GetChild(0).GetComponentInChildren<Image>().sprite = FolderSprite;
            go.GetComponent<Button>().onClick.AddListener(() => GotoDirectory(dir));
        }

        foreach (var file in files)
        {
            if (Path.GetExtension(file) != "." + CurrentFileExt)
            {
                continue;
            }

            GameObject go = Instantiate(FileItem, FileRoot);
            go.GetComponentInChildren<TMP_Text>().text = Path.GetFileName(file);
            go.transform.GetChild(0).GetComponentInChildren<Image>().sprite = FileSprite;

            if (SaveMode)
            {
                go.GetComponent<Button>().interactable = false;
            }
            else
            {
                go.GetComponent<Button>().onClick.AddListener(() => SelectFile(file));
            }
        }
    }

    private void SelectFile(string path)
    {
        CurrentSelectAction.Invoke(path);
        
        UIRoot.SetActive(false);
    }

    public void SaveFile()
    {
        CurrentSelectAction.Invoke(PathInput.text + "/" + FileNameInput.text);
        
        UIRoot.SetActive(false);
    }

    public void Cancel()
    {
        CurrentCancelAction.Invoke();
        
        UIRoot.SetActive(false);
    }

    public void OpenSelectFileBrowser(string fileExt, Action<string> selectAction, Action cancelAction)
    {
        SaveMode = false;
        
        CurrentFileExt = fileExt;
        CurrentSelectAction = selectAction;
        CurrentCancelAction = cancelAction;

        TitleText.text = "Select " + fileExt;
        
        UIRoot.SetActive(true);
        SelectFileUI.SetActive(true);
        SaveFileUI.SetActive(false);
    }

    public void OpenSaveFileBrowser(string fileExt, Action<string> selectAction, Action cancelAction)
    {
        SaveMode = true;
        
        CurrentFileExt = fileExt;
        CurrentSelectAction = selectAction;
        CurrentCancelAction = cancelAction;

        TitleText.text = "Save " + fileExt;
        
        UIRoot.SetActive(true);
        SelectFileUI.SetActive(false);
        SaveFileUI.SetActive(true);
    }

    private void Update()
    {
        if (UIRoot.activeSelf)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if (EventSystem.current.currentSelectedGameObject == PathInput.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    
                    GotoDirectory(PathInput.text);
                }
                else if (EventSystem.current.currentSelectedGameObject == FileNameInput.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    
                    SaveFile();
                }
            }
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}

public class BeanFileBrowserQuickAccessItem
{
    public BeanFileBrowserQuickAccessItem(string Name, string Path)
    {
        this.Name = Name;
        this.Path = Path;
    }
    
    public string Name;
    public string Path;
}