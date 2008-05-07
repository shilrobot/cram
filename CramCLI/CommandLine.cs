using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Cram
{
    public class CommandLine
    {
        public static void Bail(string fmt, params object[] args)
        {
            System.Console.WriteLine(fmt, args);
            System.Environment.Exit(1);
        }

        public static void BailWithUsage(string fmt, params object[] args)
        {
            System.Console.WriteLine(fmt, args);
            ShowUsage();
            System.Environment.Exit(1);
        }

        public static void ShowUsage()
        {
            System.Console.WriteLine("Usage: CramCLI.exe [options] SOURCE IMAGES...");
            System.Console.WriteLine("Packs multiple sprite images into a small set of pages.");
            System.Console.WriteLine();
            System.Console.WriteLine("Options:");
            System.Console.WriteLine("  -bg [AA]RRGGBB          Changes background color");
            System.Console.WriteLine("  -border PIXELS          Sets padding between sprite frames");
            System.Console.WriteLine("  -clean                  Delete all pages with the same base name before");
            System.Console.WriteLine("                          generating any new ones.");
            System.Console.WriteLine("  -colorkey MODE          Defines how to perform colorkeying.");
            System.Console.WriteLine("                            none: Don't do any extra colorkeying");
            System.Console.WriteLine("                            auto: Detect colorkey from corners of images");
            System.Console.WriteLine("                            [AA]RRGGBB: Colorkey using this color");
            System.Console.WriteLine("  -config CRAMFILE        Discards earlier arguments and loads settings");
            System.Console.WriteLine("                          and image list from the specified .cram file.");
            System.Console.WriteLine("  -crop                   Crops whitespace from around sprites");
            System.Console.WriteLine("  -fallbacks COUNT        Sets number of fallbacks to try. 0 = Disable");
            System.Console.WriteLine("  -fbsizes WxH,WxH,...    Sets fallback sizes to try");
            System.Console.WriteLine("  -ipath MODE[:PATH]      Defines how to save image paths in the XML file.");
            System.Console.WriteLine("                            short: Just use base filename.");
            System.Console.WriteLine("                            full: Use full pathname");
            System.Console.WriteLine("                            rel:PATH: Use pathnames relative to PATH.");
            System.Console.WriteLine("  -o XMLFILE              REQUIRED: Sets the output XML file path");
            System.Console.WriteLine("  -rotate                 Rotates sprites to save space");
            System.Console.WriteLine("  -size WxH               Sets the page size to use");
            System.Console.WriteLine("  -unique                 Removes duplicate frames");
            System.Console.WriteLine("  -v, -version            Displays version information and exits");
            System.Console.WriteLine("  -?, -help               Displays help information and exits");
        }

        public static int Run(string[] stringArgs)
        {
            CramSettings settings = new CramSettings();

            Queue<String> queue = new Queue<String>(stringArgs);
            bool outputSpecified = false;

            while (queue.Count > 0)
            {
                string arg = queue.Dequeue();
                switch (arg)
                {
                    case "-o":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -o");
                            settings.XmlFilename = queue.Dequeue();
                            outputSpecified = true;
                        }
                        break;

                    case "-colorkey":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -colorkey");
                            string param = queue.Dequeue();
                            Color specificColor;
                            if (param == "auto")
                            {
                                settings.ColorKeyMode = ColorKeyMode.Automatic;
                            }
                            else if (param == "none")
                            {
                                settings.ColorKeyMode = ColorKeyMode.Disabled;
                            }
                            else if (Util.ParseColor(param, out specificColor))
                            {
                                settings.ColorKeyMode = ColorKeyMode.Manual;
                                settings.ColorKeyColor = specificColor;
                            }
                            else
                                Bail("Invalid parameter for -colorkey: {0}", param);
                        }
                        break;

                    case "-size":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -size");
                            string param = queue.Dequeue();
                            if (!Util.ParseSize(param, out settings.PageSize))
                                Bail("Invalid parameter for -size: {0}", param);
                        }
                        break;

                    case "-bg":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -bg");
                            string param = queue.Dequeue();
                            if (!Util.ParseColor(param, out settings.BackgroundColor))
                                Bail("Invalid parameter for -bg: {0}", param);
                        }
                        break;

                    case "-border":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -border");
                            string param = queue.Dequeue();
                            int size;
                            if (!Int32.TryParse(param, out size) || size < 0)
                                Bail("Invalid parameter for -border: {0}", param);
                            settings.Border = size;
                        }
                        break;

                    case "-fallbacks":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -fallbacks");
                            string param = queue.Dequeue();
                            int size;
                            if (!Int32.TryParse(param, out size) || size < 0)
                                Bail("Invalid parameter for -fallbacks: {0}", param);
                            if (size == 0)
                            {
                                settings.AllowFallbacks = false;
                                settings.MaxFallbacks = 0;
                            }
                            else
                            {
                                settings.AllowFallbacks = true;
                                settings.MaxFallbacks = size;
                            }
                        }
                        break;

                    case "-fbsizes":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -fbsizes");
                            string param = queue.Dequeue();
                            if (!Util.ParseSizeList(param, out settings.FallbackSizes) || settings.FallbackSizes.Count < 0)
                                Bail("Invalid parameter for -fbsizes: {0}", param);
                        }
                        break;

                    case "-crop":
                        settings.Crop = true;
                        break;

                    case "-rotate":
                        settings.Rotate = true;
                        break;

                    case "-unique":
                        settings.Unique = true;
                        break;

                    case "-?":
                    case "-help":
                    case "-h":
                    case "--help":
                        ShowUsage();
                        System.Environment.Exit(0);
                        break;

                    case "-v":
                    case "-version":
                        Console.WriteLine("Sprite Crammer {0}", VersionInfo.Version);
                        Console.WriteLine("Copyright (C) 2008 Scott Hilbert");
                        Console.WriteLine("http://www.shilbert.com/");
                        System.Environment.Exit(0);
                        break;

                    case "-ipath":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -ipath");
                            string param = queue.Dequeue();
                            string paramLower = param.ToLower();
                            if (paramLower == "short")
                                settings.PathMode = PathMode.Short;
                            else if (paramLower == "full")
                                settings.PathMode = PathMode.Full;
                            else if (paramLower.StartsWith("rel:") && param.Length > 4)
                            {
                                settings.PathMode = PathMode.Relative;
                                settings.RelativePathBase = param.Substring(4);
                            }
                            else
                                Bail("Invalid parameter for -ipath: {0}", param);
                        }
                        break;

                    case "-config":
                        {
                            if (queue.Count == 0 || queue.Peek().StartsWith("-"))
                                Bail("Missing parameter for -config");
                            string param = queue.Dequeue();
                            settings = CramSettings.Load(param);
                            outputSpecified = true;
                            if (settings == null)
                                Bail("Cannot load config file: {0}", param);
                        }
                        break;

                    case "-clean":
                        settings.Clean = true;
                        break;

                    default:
                        if (arg.StartsWith("-"))
                        {
                            Console.WriteLine("Unknown option: {0}", arg);
                            System.Environment.Exit(0);
                        }
                        else
                            settings.SourceImages.Add(arg);
                        break;
                }
            }

            settings.FallbackSizes.Sort(Util.CompareFallbackSizes);

            if (!outputSpecified)
            {
                BailWithUsage("No output file specified. Use -o to specify output file path.");
            }

            SpriteCrammer crammer = new SpriteCrammer();
            crammer.Settings = settings;
            crammer.LogEvent += new ProgressLog(crammer_LogEvent);

            bool result = crammer.Run();

            return result ? 0 : 1;
        }

        static void crammer_LogEvent(string s)
        {
            Console.WriteLine(s);
        }
    }
}
