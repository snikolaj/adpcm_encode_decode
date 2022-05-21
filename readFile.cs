// See https://aka.ms/new-console-template for more information
using System.Text;

string loadedFileName = "C:/Users/Stefan/Desktop/chhl.wav";

byte[] returnFile(string fileName)
{
    if (File.Exists(fileName))
    {
        using (FileStream fs = File.OpenRead(fileName))
        {
            // UTF8Encoding temp = new UTF8Encoding(true);
            byte[] data = new byte[fs.Length];
            // async read, modify if file is too big
            while (fs.Read(data, 0, data.Length) > 0)
            {
                // Console.WriteLine(temp.GetString(data));
            }
            return data;
        }
    }
    else
    {
        return Encoding.ASCII.GetBytes("File doesn't exist");
    }
}

byte[] data = returnFile(loadedFileName);



string testWAV(byte[] wavFile)
{
    if(wavFile == null || wavFile.Length < 45)
    {
        return "Invalid wave file";
    }
    string returnString = "";

    returnString += String.Format("Header: {0}{1}{2}{3}\n", (char)wavFile[0], (char)wavFile[1], (char)wavFile[2], (char)wavFile[3]);

    returnString += String.Format("Size: {0} bytes\n", (wavFile[7] << 24) + (wavFile[6] << 16) + (wavFile[5] << 8) + wavFile[4]);

    returnString += String.Format("File type: {0}{1}{2}{3}\n", (char)wavFile[8], (char)wavFile[9], (char)wavFile[10], (char)wavFile[11]);

    returnString += String.Format("Format marker: {0}{1}{2}\n", (char)wavFile[12], (char)wavFile[13], (char)wavFile[14]);

    int formatLength = wavFile[16] + (wavFile[17] << 8) + (wavFile[18] << 16) + (wavFile[19] << 24);

    returnString += String.Format("Format size: {0}\n", formatLength);

    returnString += String.Format("Type of format: {0}\n", wavFile[20] + (wavFile[21] << 8));

    int numberOfChannels = wavFile[22] + (wavFile[23] << 8);
    returnString += String.Format("Number of channels: {0}\n", numberOfChannels);

    int sampleRate = wavFile[24] + (wavFile[25] << 8) + (wavFile[26] << 16) + (wavFile[27] << 24);
    returnString += String.Format("Sample rate: {0}\n", sampleRate);

    int Bps = wavFile[28] + (wavFile[29] << 8) + (wavFile[30] << 16) + (wavFile[31] << 24);
    int calculatedBitsPerSample = Bps / (sampleRate * numberOfChannels / 8);
    returnString += String.Format("Bytes per second: {0}\n", Bps);

    int blockAlign = wavFile[32] + (wavFile[33] << 8);
    int calculatedBlockAlign = (numberOfChannels * calculatedBitsPerSample / 8);
    returnString += String.Format("Bytes per sample (all channels): {0}\n", blockAlign);

    int bitsPerSample = wavFile[34] + (wavFile[35] << 8);
    returnString += String.Format("Bits per sample: {0}\n", bitsPerSample);

    if(calculatedBitsPerSample != bitsPerSample || calculatedBlockAlign != blockAlign)
    {
        returnString += "Incorrect bits per sample or block align, data may be corrupted\n";
    }

    returnString += String.Format("Data subchunk header: {0}{1}{2}{3}\n", (char)wavFile[36], (char)wavFile[37], (char)wavFile[38], (char)wavFile[39]);

    int subChunkSize = wavFile[40] + (wavFile[41] << 8) + (wavFile[42] << 16) + (wavFile[43] << 24);
    returnString += String.Format("Subchunk size: {0}", subChunkSize);
    
    return returnString;
}

Console.WriteLine(testWAV(data));
