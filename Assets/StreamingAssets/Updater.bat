@echo off
title Updater
echo DO NOT RUN IN EDITOR.
pause
echo Please wait...
echo Ending game process
taskkill /f /im Cookieclicker2.mp4.exe
echo Moving update file
move Update.7z ..\..\
echo Moving 7z
move 7z.exe ..\..\
echo Extracting the update file
cd ..
cd ..
echo Deleting game files
del Cookieclicker2.mp4.exe
del UnityCrashHandler64.exe
del UnityCrashHandler32.exe
rd /s /q Cookieclicker2.mp4_Data
rd /s /q MonoBleedingEdge
del UnityPlayer.dll
7z.exe x Update.7z
echo Deleting temp files
del 7z.exe
del Update.7z
echo Running game
Cookieclicker2.mp4.exe
:https://www.googleapis.com/drive/v3/files/1MuanUdc7d91tu-h-l4mdAZwsFJPQAYoC?alt=media&key=AIzaSyAWPHSBM-NDHRnGNvOaw09Y_zmzW2Qizxk