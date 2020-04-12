using mitoSoft.Graphs.GraphVizInterop.Enums;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public class ImageRenderer
    {
        private readonly string _graphVizFolder;

        public ImageRenderer(string graphVizBinPath)
        {
            _graphVizFolder = graphVizBinPath;
        }

        /// <summary>
        /// Generates a 'System.Drawing' image form the 'dotText' parameter
        /// </summary>
        public Image RenderImage(string dotText, LayoutEngine layoutEngine = LayoutEngine.dot, ImageFormat imageFormat = ImageFormat.png)
        {
            var lines = dotText.Split(Environment.NewLine.ToCharArray()).ToList();

            return this.RenderImage(lines, layoutEngine, imageFormat);
        }

        /// <summary>
        /// Generates a 'System.Drawing' image form the 'dotText' parameter
        /// </summary>
        public Image RenderImage(List<string> dotText, LayoutEngine layoutEngine = LayoutEngine.dot, ImageFormat imageFormat = ImageFormat.png)
        {
            var tempFileCollection = new TempFileCollection();

            var dotFile = InitializeRendering(dotText, tempFileCollection);

            var imageFile = this.GetNextFileName(Path.Combine(Path.GetTempPath(), "Graphviz#.png"));
            tempFileCollection.AddFile(imageFile, false);

            this.RunGraphViz(dotFile, imageFile, layoutEngine, "T" + imageFormat.ToString());

            var image = Image.FromFile(imageFile);

            tempFileCollection.Delete();

            return image;
        }

        public void RenderImage(string dotText, string imageFile, LayoutEngine layoutEngine = LayoutEngine.dot, ImageFormat imageFormat = ImageFormat.png)
        {
            var lines = dotText.Split(Environment.NewLine.ToCharArray()).ToList();

            this.RenderImage(lines, imageFile, layoutEngine, imageFormat);
        }

        public void RenderImage(List<string> dotText, string imageFile, LayoutEngine layoutEngine = LayoutEngine.dot, ImageFormat imageFormat = ImageFormat.png)
        {
            var tempFileCollection = new TempFileCollection();

            var dotFile = InitializeRendering(dotText, tempFileCollection);

            this.RunGraphViz(dotFile, imageFile, layoutEngine, "T" + imageFormat.ToString());

            tempFileCollection.Delete();
        }

        private string InitializeRendering(IEnumerable<string> dotText, TempFileCollection tempFileCollection)
        {
            var dotLines = dotText.ToList();

            dotLines.RemoveAll(s => s == string.Empty);

            var tempPath = Path.GetTempPath();

            this.TryToDeleteOldTmp(tempPath);

            var dotFile = this.GetNextFileName(Path.Combine(tempPath, "Graphviz#.txt"));
            tempFileCollection.AddFile(dotFile, false);
            File.WriteAllLines(dotFile, dotLines);

            return dotFile;
        }

        private void RunGraphViz(string dotFile, string imageFile, LayoutEngine layoutEngine, string outputFomat)
        {
            if (!Directory.Exists(_graphVizFolder))
            {
                throw new DirectoryNotFoundException("Please install GraphViz and asign the installation folder!");
            }

            var prop = new ProcessStartInfo
            {
                FileName = Path.Combine(_graphVizFolder, layoutEngine.ToString() + ".exe"),
                Arguments = "-" + outputFomat + " \"" + dotFile + "\" -o \"" + imageFile + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Process process = Process.Start(prop);

            string errorText = process.StandardError.ReadToEnd();
            if (errorText != null && errorText.Contains("syntax error"))
            {
                throw new FormatException(errorText);
            }
            else if (errorText != null && errorText.Contains("Error:"))
            {
                throw new InvalidOperationException(errorText);
            }

            process.WaitForExit(120000);
        }

        /// <summary>
        /// Delete old 'unused' files
        /// </summary>
        private void TryToDeleteOldTmp(string directoryName)
        {
            try
            {
                foreach (var file in Directory.GetFiles(directoryName))
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Name.Contains("Graphviz#"))
                    {
                        try
                        {
                            fileInfo.Delete();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException) { }
        }

        /// <summary>
        /// Generates a unique filename via a number postfix
        /// </summary>
        private string GetNextFileName(string fileName)
        {
            var tmp = new FileInfo(fileName);
            for (int i = 1; i <= 100; i++)
            {
                var path = Path.Combine(tmp.DirectoryName, tmp.Name.Replace(tmp.Extension, "") + i + tmp.Extension);
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists == false)
                {
                    return fileInfo.FullName;
                }
            }
            throw new InvalidOperationException("No temporary files awailable");
        }
    }
}