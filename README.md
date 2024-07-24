# UIPanels

Small utility to comfortly create and manage UIPanels with animations and states

Requires DoTween for animations to work

# Usage
  - Download and add to your project [UIPanel.cs](Code/UIPanel.cs), [UIPanelsController.cs](Code/UIPanelsController.cs) and [Singleton.cs](Code/Singleton.cs)
  - Add [UIPanelsController.cs](Code/UIPanelsController.cs) to your Canvas on scene (it doesnt have to be on canvas, its just a habit of mine)
  - Create and layout a panel (For example Main Menu panel)
  - Add your panel to UIPanelsController's [MainPanelType](Code/UIPanelsController.cs/#L99) enum
  - Add your panel, aswell as the type of it into UIPanelsController's mainPanel list in your scene
  - Invoke [ShowMainPanel](Code/UIPanelsController.cs/#L67) method to show panel

# Notes
  - There is 2 types of panels, Main Panels and Misc Panels, the difference is that Misc Panels will try to open BackgroundPanel with them (for example background blur, darkening)
  - When showing Main Panel it will close every other Main Panel that is opened, same is applied to Misc Panels
  - Main Panels will not close any MiscPanels and vice versa
  - UIPanel is an Abstract class, you can inherit any new panels from it, to add some logic (animations, buttons) when the panel is showing, or to change animations completly
