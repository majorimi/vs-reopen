# Visual Studio Document Reopen
Visual Studio Extension to reopen the last closed document(s) with (CTRL + SHIFT + T) shortcut.

Download from [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=major.VSDocumentReopen).

## Version history

##### v. 1.4.8
- Logging for issue investigation: "System.ArgumentException: The path is not of a legal form."

##### v. 1.4.7
- Bug fix: Async package loading issue. Extension does not work if VS was started by double clicking on a .sln file.

##### v. 1.4.6
- Code improvements: 
	- Nuget package updates,
	- Async package loading
- Enhance History Tool Window:
	- Remove non existing items from history toolbar menu button

##### v. 1.4.5
- Tool Window issue fixes:
	- Search result label length issue
	- Dark theme support
- Visual Studio 2019 upgrade

##### v. 1.4.4
- Tool Window issue fixes:
	- #11: Tool bar icons vanish if the tool window gets too narrow
	- Column reorder does not work
- Tool Window columns:
  - Can be show/hide form context menu
  - Order and size customizations saved and restored
  - Reset customization from context menu
- Memory usage diagnostics 

##### v. 1.4.3
- Emergency update due to key binding issues, add more diagnostics.

##### v. 1.4.2
- Bug fix: History Tool Window search box does not raise search event for Backspace
- Bud fix: set key bindings throws ArgumentException for localized Visual Studio instances 

##### v. 1.4.1
- History Tool Window supports Visual Studio color theme
- Store search history
- Code enhancements and more detailed diagnostics

#### v. 1.4.0
- **Move menu commands from "Tools" to "File" menu!**
- New menu command: Remove last closed document from history without open it (CTRL + SHIFT + D)
- Remove history duplications
- Add logging and diagnostics
- Code enhancements

##### v. 1.3.1
- Order history data by clicking on column headers in History Tool Window
- Code enhancements, small issue fixes

#### v. 1.3.0
- Add support for **Visual Studio 2015**
- Enhance History Tool Window
	- Show Solution Explorer style document Icon in "Type" column
	- Show icon in "Exists" column

#### v. 1.2.0
- Enhance History Tool Window:
	- Reopen any closed document by double clicking or selected document(s) using toolbar menu button
	- Remove selected document(s) using toolbar menu button or Delete key
	- Clear history with toolbar menu button
	- Filter history by document name or path
	- More info about the documents
- Code enhancements, small issue fixes

##### v. 1.1.1
- Enforce key bindings for menu commands
- Enable vertical scroll in History Tool Window
- Bugfix: History Tool Window does not initialized with history data
	
#### v. 1.1.0
- Reorganized menu command: under the Tools menu child menu
- Track the last 5 closed document in child menu
- New menu command: Show all history in a new Tool Window (CTRL + SHIFT + R)
- Persist and load closed documents history (per solution)
- Clear history functionality
- New Preview image

##### v. 1.0.1
- Bug fix: exception when file was deleted
- New Icon and Preview images

#### v. 1.0.0
- Initial release. Extension add new command to Tools menu and CTRL+SHIFT+T shortcut
