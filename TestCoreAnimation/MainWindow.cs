using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreAnimation;
using MonoMac.CoreGraphics;

namespace TestCoreAnimation
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		NSWindow overlay;
		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			var mainView = new NSView () {
				Frame = ContentView.Frame
			};
			var button = new NSButton () {
				StringValue = "Click Me",
				Frame = new System.Drawing.RectangleF (40, 40, 90, 30),
				BezelStyle = NSBezelStyle.Rounded
			};
			mainView.AddSubview (button);
			button.Activated += (object sender, EventArgs e) => {
				NSButton nsbutton = (NSButton)sender;
				var win = nsbutton.Window;

				if (overlay == null)
				{
					overlay = new NSWindow ();
					overlay.SetExcludedFromWindowsMenu (true);
					overlay.StyleMask = NSWindowStyle.Borderless;
					var childButton = new NSButton () {
						Title = "FOOBAR",
						Frame = new RectangleF (10, 10, 150, 30),
						BezelStyle = NSBezelStyle.Rounded
					};
					overlay.ContentView.AddSubview (childButton);

					var rectf = new RectangleF ();
					rectf.Location = win.ConvertBaseToScreen (new PointF (80, 80));
					rectf.Size = new SizeF (200, 200);

					overlay.SetFrame (rectf, false);
					overlay.BackgroundColor = NSColor.Cyan;

					win.AddChildWindow (overlay, NSWindowOrderingMode.Above);
				} else {
					win.RemoveChildWindow (overlay);
					overlay.Dispose ();
					overlay = null;
				}
			};

			ContentView.AddSubview (mainView);
		}

		#endregion
	}
}

