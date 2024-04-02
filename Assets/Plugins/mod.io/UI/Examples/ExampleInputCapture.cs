using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Example script on how to setup the key/button bindings for the ModIO Browser. Inputs such as
/// Tab left and right, options and alternate-submit. All of these inputs provide extra functionality
/// and ease of navigation for the user.
///
/// This script makes use of the InputReceiver static class to invoke the correct behaviour on the
/// Browser.
/// For example: when the input is captured for KeyCode.Joystick1Button2, the method on InputReceiver.Alternate()
/// is invoked. You can use InputReceiver.cs to tell the browser when a specific input has been used
/// </summary>
public class ExampleInputCapture : MonoBehaviour
{    
    // Control mappings for keyboard, controller, and mouse. If the controls for the app
    // are changed under: Project Settings -> Input -> Axes, then they must be also be changed here!
    // Unfortunately as of developing this, there is no simple way to fetch these values by code.
    public List<string> controllerAndKeyboardInput = new List<string>();
    public List<string> mouseInput = new List<string>();
    public string verticalControllerInput = "Vertical";

}