## WPF Caliburn Micro + AvalonDock 2.0 Template Guide

This project was born after [this post of mine](http://stackoverflow.com/questions/28194046/correctly-handling-document-close-and-tool-hide-in-a-wpf-app-with-avalondockcal). I have spent a lot of time googling around and collecting issues and potential solutions for letting a WPF application work with Caliburn Micro and AvalonDock 2.0 together, even if this is the most basic scenario. Having found a lot of unanswered questions about this topic, I thought this might at least help someone else who is starting with these two libraries.

A big thank you to all the community posters and bloggers whose suggestions were useful for building this even raw but somewhat hopefully functional template. *Sumus enim nani super humeros gigantum...*.

In this document you can find a step by step guide for creating the essential WPF application skeleton based onto AD+CM contained in this code repository. That's just a starting point, intentionally kept as simple as possible, but it would be nice if the community could improve it so we can have a guide for implementing such a common mix.

What follows is a recap of the main steps for building this app template. Don't expect to find any detail here; its main purpose is helping a reader understand how this was built by introducing a component at a time so that each dependency is discussed before it is required. At any rate, you will find comments and even some links wherever useful in the code itself.

See the isses section below for a list of current issues. In the code, each piece of hackish code is marked by `@@hack`.

### 1. Basic Procedure

1. create a new WPF app. Create folders for `Messages`, `ViewModels`, `Views`. I prefer having each VM in its own subfolder together with all its non shared dependencies, so I add a subfolder (which is **not** a namespace provider) in the `ViewModels` folder for each VM.

2. add `Caliburn.Micro` and `AvalonDock` from NuGet.

3. add reference to `System.ComponentModel.Composition` for MEF.

4. add an empty `IShell` internal interface and the MEF bootstrapper (see `MefBootstrapper.cs`). Customize its `Configure` method as needed so that your objects can be instantiated by the IoC container. Note that this bootstrapper also uses a `DebugLogger` in the DEBUG profile, and an `ApplicationViewModel` I use to represent the shared application status. In turn, this exposes a `Settings` property which wraps the WPF application settings to be accessed by other VMs in the application.

5. add the required app settings: for my purposes they are at least `Culture` to define the UI culture, `MaxOpenDocuments` to set a limit to the maximum number of opened documents (0=no limit), and `MainWidth`, `MainHeight` and `IsAutoLayoutRestoreEnabled` to handle layout restore in the main window.
        
6. in `App.xaml` remove the `StartupUri` attribute and paste this code in the root element:

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary>
                    <local:MefBootstrapper x:Key="bootstrapper" />
                </ResourceDictionary>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

7. Remove `MainWindow` from the project.

8. Add `DocumentBase.cs` and `ToolBase.cs` to the `ViewModels` folder. These are the base viewmodels for documents (to handle close) and tools (to handle show/hide and show an icon).

9. Add `MainViewModel.cs` into `ViewModels`. This is the main, shell VM derived from `Conductor<IScreen>.Collection.OneActive`, and containing all the VMs used in the application in its `Tools` or `Documents` collections.

10. Add a new WPF window in `Views` named `MainView` for the corresponding view.

11. Add the autobinder template selector (see [this post](http://stackoverflow.com/questions/14546583/avalondock-2-with-caliburn-micro) and [this discussion](https://caliburnmicro.codeplex.com/discussions/430994)).

## Issues

I'm numbering the issues so we can easily refer to them.

###1 Closing Documents

Status: apparently solved with a hack.

The first issue comes when handling document-pane close. AD has its document handling mechanism, which should be synchronized with CM's one. 

CM is based on a screen conductor; when a screen needs to be closed, the method `TryClose` is used to close it if possible (i.e. unless a guard method tells the framework that the screen cannot be closed, e.g. because the document is dirty). 

To let AD play with CM I'm using a workaround similar to that described in [Prevent document from closing in DockingManager](http://stackoverflow.com/questions/17185780/prevent-document-from-closing-in-dockingmanager?rq=1), where the main view code directly calls this method handling the docking manager closing event: when AD is closing the document, call the underlying VM guard method and cancel if the VM says the document is dirty; if not cancelled, then AD goes on closing, thus firing the `DocumentClosed` event. At this stage, tell the VM it must close anyway.

###2. Closing Tools

Status: apparently solved with a hack.

In AD, tools panes can be only hidden, not closed (as document panes), by the user. I'd like the user to be able to hide them, but of course also to restore them when required. 

I thus added a checkable menu item under a the `View` menu for my tool pane, bound to a visible property of the corresponding VM. As CM does not have such property in its `Screen`, I'm deriving a `ToolBase` from it like (meanwhile it's also useful to add an icon to each tool pane).

A potential issue arises with this approach: if I hide the tool pane by clicking on its X button (or by unchecking its menu item), I can see that my VM `IsVisible` property is set to false as expected, and the pane is hidden. Then, if I programmatically set this property back to true (by checking the corresponding menu item) the pane is not shown. Often, a null object reference exception is thrown. The explanation of this lays in the fact that AD is actually closing rather than hiding the control, and this depends on the fact that the boolean to visibility converter used to bind the VM `IsVisible` property to the AD visibility is returning (as expected) `Collapsed` for false, when instead AD is expecting `Hidden`. This is explained in [this post](http://stackoverflow.com/questions/23617707/binding-to-layoutanchorableitem-visibility-in-avalondock-2). The suggested solution seems not to work with the current implementation of that converter, so I simply added a custom one (`AdBooleanToVisibilityConverter`).

###3. Saving/Restoring Layout

To save/restore the layout, I find satisfying to just use code behind, as this is essentially view-related stuff, not pertaining to the VM. So I just add a couple of menu items and their handlers. See the `MainView` class in the project.

You can use a couple of menu items in the main view to save or restore the layout once you have modified it at your will.
