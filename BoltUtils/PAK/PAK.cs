using BoltUtils.PAK.GIM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yarhl.IO;

namespace BoltUtils.PAK
{
    public class PAK
    {
        private string PakName;
        private List<byte[]> PakFiles = new List<byte[]>();
        private List<PakContentFormats.Format> FileFormat = new List<PakContentFormats.Format>();
        private List<GimData> FileData = new List<GimData>();


        public PAK(string pakname)
        {
            PakName = pakname;
        }


        public void AddFile(byte[] file)
        {
            PakFiles.Add(file);
            PakContentFormats.Format format = CheckFileFormat(file);
            FileFormat.Add(format);
            if (format == PakContentFormats.Format.GIM)
            {
                GimData data = GimData.GetGimFileInfo(file);
                FileData.Add(data);
            }
            else
            {
                FileData.Add(null);
            }
        }


        private static PakContentFormats.Format CheckFileFormat(byte[] file)
        {
            using (var stream = DataStreamFactory.FromStream(new MemoryStream(file)))
            {
                var reader = new DataReader(stream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                try
                {
                    byte[] magic = reader.ReadBytes(0x8);
                    foreach (PakContentFormats.Format format in Enum.GetValues(typeof(PakContentFormats.Format)))
                    {
                        if (magic.SequenceEqual(PakContentFormats.MagicBytes[format]))
                        {
                            return format;
                        }
                    }
                    return PakContentFormats.Format.UNKNOWN;
                }
                catch (Exception)
                {
                    Console.WriteLine("CheckFileFormat() failed.");
                    return PakContentFormats.Format.UNKNOWN;
                }
            }
        }


        /// <summary>
        /// Gets the file info for the GIMs in the PAK.
        /// </summary>
        /// <returns>A GimData List.</returns>
        public List<GimData> GetFileData()
        {
            return FileData;
        }


        /// <summary>
        /// Gets the file formats for the files in the PAK.
        /// </summary>
        /// <returns></returns>
        public List<PakContentFormats.Format> GetFileFormats()
        {
            return FileFormat;
        }


        /// <summary>
        /// Gets the file format for the file matching the given index.
        /// </summary>
        /// <returns></returns>
        public PakContentFormats.Format GetFileFormat(int index)
        {
            return FileFormat[index];
        }


        /// <summary>
        /// Gets the PAK filename.
        /// </summary>
        /// <returns></returns>
        public string GetPAKName()
        {
            return PakName;
        }


        /// <summary>
        /// Gets the files in the PAK.
        /// </summary>
        /// <returns></returns>
        public List<byte[]> GetPAKContents()
        {
            return PakFiles;
        }


        public int GetPAKFileAmount()
        {
            return PakFiles.Count;
        }


        /// <summary>
        /// Returns a string with the file's name preceded by its index inside the PAK.
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        public string GetFilenameForExport(int fileIndex)
        {
            string filename = "unknown_name";
            if (FileFormat[fileIndex] == PakContentFormats.Format.GIM)
            {
                filename = FileData[fileIndex].Filename;
                if (filename == "No Data" || String.IsNullOrWhiteSpace(filename))
                {
                    filename = "unknown_texture_name";
                    filename += String.Format(".{0}", PakContentFormats.Extension[FileFormat[fileIndex]]);
                }
            }

            else if (FileFormat[fileIndex] == PakContentFormats.Format.MB_PSP_GDE || FileFormat[fileIndex] == PakContentFormats.Format.MB_PS2_GDE)
            {
                filename = "unknown_model_name";
                filename += String.Format(".{0}", PakContentFormats.Extension[FileFormat[fileIndex]]);
            }

            else
            {
                filename += String.Format(".{0}", PakContentFormats.Extension[FileFormat[fileIndex]]);
            }

            return String.Format("{0} - {1}", fileIndex.ToString().PadLeft(3, '0'), filename);
        }


        public void ExportFile(int fileIndex, string targetPath)
        {
            File.WriteAllBytes(targetPath, PakFiles[fileIndex]);
        }


        public void ReplaceFile(int fileIndex, byte[] fileBytes)
        {
            PakFiles[fileIndex] = fileBytes;
            PakContentFormats.Format format = CheckFileFormat(fileBytes);
            FileFormat[fileIndex] = format;
            if (format == PakContentFormats.Format.GIM)
            {
                GimData data = GimData.GetGimFileInfo(fileBytes);
                FileData[fileIndex] = (data);
            }
            else
            {
                FileData[fileIndex] = null;
            }
        }

    }
}
