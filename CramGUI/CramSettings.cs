using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace Cram
{
    public enum ColorKeyMode
    {
        Manual,
        Automatic,
        Disabled
    }

    public enum PathMode
    {
        Short,
        Full,
        Relative
    }

    public class CramSettings
    {
        public List<string> SourceImages = new List<string>();

        public ColorKeyMode ColorKeyMode = ColorKeyMode.Disabled;

        [XmlIgnore]
        public Color ColorKeyColor = Color.White;

        [XmlElement("ColorKeyColor")]
        public string ColorKeyColorString 
        {
            get { return Util.FormatColor(ColorKeyColor); }
            set
            {
                if (!Util.ParseColor(value, out ColorKeyColor))
                    ColorKeyColor = Color.White;
            }
        }

        public Size PageSize = new Size(512, 512);

        [XmlIgnore]
        public Color BackgroundColor = Color.Transparent;

        [XmlElement("BackgroundColor")]
        public string BackgroundColorString
        {
            get { return Util.FormatColor(BackgroundColor); }
            set
            {
                if (!Util.ParseColor(value, out BackgroundColor))
                    BackgroundColor = Color.White;
            }
        }

        public int Border = 0;

        public bool AllowFallbacks = false;
        public int MaxFallbacks = 0;
        public List<Size> FallbackSizes = new List<Size>();

        public bool Crop = false;
        public bool Unique = false;
        public bool Rotate = false;

        public string XmlFilename = "";

        public PathMode PathMode = PathMode.Short;
        public string RelativePathBase = "";

        public bool Clean = false;
        
        public static CramSettings Load(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CramSettings));
                FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                CramSettings settings = (CramSettings)serializer.Deserialize(stream);
                stream.Close();
                return settings;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Save(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CramSettings));
                FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                serializer.Serialize(stream, this);
                stream.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
