using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RestoreOriginal
{
    class ProcessFile
    {
        static int int16(byte [] bytes, int offset){
            return bytes[offset] | (bytes[offset+1]<<8);
        }

        static int int32(byte [] bytes, int offset){
            return bytes[offset] | (bytes[offset+1]<<8) | (bytes[offset+2]<<16) | (bytes[offset+3]<<24);
        }

        int nFields;
        int nEntries;
        string [] fieldNames;
        int [] fieldTypes;
        string [,] data;
        int    [,] dataOffsets;

        public void read(string fname){
            byte[] fileBytes = File.ReadAllBytes(fname);

            nFields = int16(fileBytes, 0);

            int offset = 2;
            fieldNames = new string[nFields];
            fieldTypes = new int[nFields];

            for(int i = 0; i < nFields; i++){
                int tag = (int) fileBytes[offset];
                int fieldNameLength = (int) fileBytes[offset + 1];
                int fieldType = (int) fileBytes[offset];
                fieldTypes[i] = fieldType;

                offset += 2;
                byte [] fieldNameBytes = new byte[fieldNameLength];
                for(int k = 0; k < fieldNameLength; k++){
                    fieldNameBytes[k] = fileBytes[offset + k];
                }

                string result = System.Text.Encoding.UTF8.GetString(fieldNameBytes);
                fieldNames[i] = result;

                Console.WriteLine("Field "+i+", name: " + result + ", type: " + fieldType);
                offset += fieldNameLength;
            }

            nEntries = int16(fileBytes, offset);
            dataOffsets = new int[nEntries, nFields];
            data = new string[nEntries, nFields];

            offset += 2;

            for(int entry = 0; entry < nEntries; entry++){
                for(int column = 0; column < nFields; column++){
                    dataOffsets[entry, column] = int32(fileBytes, offset);
                    offset += 4;
                }
            }

            offset += 4; // why?

            Console.WriteLine(dataOffsets[nEntries-1, nFields-1]);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"test.txt"))
            {
                for(int entry = 0; entry < nEntries; entry++){
                    file.WriteLine("[" + entry + "]");
                    for(int column = 0; column < nFields; column++){
                        String s = "undefined";
                        if(fieldTypes[column] == 1){ // string}
                            s = getString(fileBytes, offset + dataOffsets[entry, column]);
                        }  else if(fieldTypes[column] == 0){ // float?
                            s = String.Format("{0:0.000000}", getFloat(fileBytes, offset + dataOffsets[entry, column]));
                        }
                        file.WriteLine("     " + fieldNames[column] + " : " + s);
                    }
                }
            } 
        }

        string getString(byte [] byteArray, int offset){
            int l = 0;
           // Console.WriteLine(offset);
            for(int i = 0; byteArray[offset + i]!='\0'; i++, l++);
            byte[] sBytes = new byte[l];
            for(int i = 0; byteArray[offset + i]!='\0'; i++) sBytes[i] = byteArray[offset + i];
            String s = System.Text.Encoding.UTF8.GetString(sBytes);

            return s;
        }

        float getFloat(byte [] byteArray, int offset){
            byte[] tmpArray = {byteArray[offset], byteArray[offset+1], byteArray[offset+2], byteArray[offset+3]};
            return System.BitConverter.ToSingle(tmpArray, 0);
        }




public bool ByteArrayToFile(string fileName, byte[] byteArray)
{
    try
    {
        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            fs.Write(byteArray, 0, byteArray.Length);
            return true;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception caught in process: {0}", ex);
        return false;
    }
}        
    }
}
