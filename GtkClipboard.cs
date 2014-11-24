using System;
using Gtk;
using System.ComponentModel;
using System.IO;

namespace GtkUtils
{
	/// <summary>GTK clipboard functions</summary>
	public static class GtkClipboard
	{
		/// <summary>Set the clipboard text</summary>
		public static void SetText(string text)
		{
			using (Clipboard cb = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)))
			{
				cb.Text = text;
			}
		}

		/// <summary>Is there text on the clipboard?</summary>
		public static bool ContainsText()
		{
			using (Clipboard cb = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)))
			{
				return cb.WaitIsTextAvailable();
			}
		}

		/// <summary>Get the clipboard text</summary>
		public static string GetText()
		{
			using (Clipboard cb = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)))
			{
				return cb.WaitForText();
			}
		}

		/// <summary>Is there an image on the clipboard?</summary>
		public static bool ContainsImage()
		{
			using (Clipboard cb = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)))
			{
				return cb.WaitIsImageAvailable();
			}
		}

		/// <summary>Get the image from clipboard</summary>
		public static System.Drawing.Image GetImage()
		{
			using (Clipboard cb = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false)))
			{
				Gdk.Pixbuf buff = cb.WaitForImage();
				TypeConverter tc = TypeDescriptor.GetConverter(typeof(System.Drawing.Bitmap));
				return (System.Drawing.Image)tc.ConvertFrom(buff.SaveToBuffer("png"));
			}
		}

		/// <summary>Get the image from a file name on the clipboard</summary>
		public static System.Drawing.Image GetImageFromText()
		{
			var stringSeparators = new string[] { Environment.NewLine };
			var paths = GetText().Split(stringSeparators, StringSplitOptions.None); 

			foreach (var path in paths)
			{
				if (File.Exists(path))
				{
					try
					{
						var bytes = File.ReadAllBytes(path);
						var buff = new Gdk.Pixbuf(bytes);
						TypeConverter tc = TypeDescriptor.GetConverter(typeof(System.Drawing.Bitmap));
						return (System.Drawing.Image)tc.ConvertFrom(buff.SaveToBuffer("png"));
					}
					catch (Exception e)
					{
						Console.Out.WriteLine("{0} is not an image file ({1}).", path, e.Message);
					}
				}
			}

			return null;
		}
	}
}

