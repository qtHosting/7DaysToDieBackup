# 7 Days to Die Backup
This application is designed to automatically back up a save once an hour while you are playing 7 Days to Die.

## Features
* Launch Game
* Back up save folder every hour as a zip file
* List Backup Folders and Locations

## To Use
* Open the Application
* Choose a folder you want to back up
* Set game launcher
* Launch game and play without fear.
* Every hour, on the hour, the application will create a unique zip 
* 
```
Example zip name: "MutantSeed16-06-15 22-00.zip"
```

## Launch 7 Days to Die
You can launch 7 days to die from within the app - click the "Launch Game" button and you will be able to navigate to your 7 Days to Die install folder. There you can select "7DaysToDie.exe". The application will remember the location the next time you hit the button.

## Modify/Edit list of backup folders
Right now, the application only removes games - modification might happen in the future. For now, if you remove a game from the backup list, you'll need to re-add it as if it were a new backup folder/location.

If you click file (Alt+F) you'll see an option that says "List Game Saves". Click that, and a new window will open up showing a treeview list of all of the folders being backed up.

The top-level entries are the directories being backed up. The backup location is the child. You can right-click on the list-item (parent or child) to remove it from the database, which will keep it from being backed up.

## Backup a Save Location
* Click "Open Folder"
* The folder-view should open to your AppData folder location. Navigate to the folder you want to back up. (The root of your save)

```
Example: C:\Users\<user>\AppData\Roaming\7DaysToDie\Saves\Random Gen\<FolderName>
```
* Click "Select Folder"
* Click "Backup Folder"
* Navigate to where you want to save your files

```
Example: \\batcomputer\public\7DaysToDieBackups
```
* Click "Select Folder"
* Click "Save"

The interface will reset after you click save. You can look in the List Games window to see the backups that you have running.

## Notes
The Application has to be running in order to back up files.
It runs under the user's credentials, so they need read-access to the folder they want to back up, and write access to the save location.
The application works on a timer that checks to see if the system time is x:00 - I haven't tested it on other environments, so please let me know if there are issues.
Be aware that the application doesn't check to see if the files have changed, it just makes a zip of the folder. This could eat up space quickly depending on the size of the folder in question.
