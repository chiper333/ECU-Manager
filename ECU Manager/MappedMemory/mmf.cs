using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ECU_Manager.MappedMemory
{
    internal class Mmf
    {

        /*
        This solution requires unsafe code (compile with /unsafe switch), but grabs a pointer to the memory directly; then Marshal.Copy can be used. This is much, much faster than the methods provided by the .NET framework. 
         
          // assumes part of a class where _view is a MemoryMappedViewAccessor object

    public unsafe byte[] ReadBytes(int offset, int num)
    {
        byte[] arr = new byte[num];
        byte *ptr = (byte*)0;
        this._view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        Marshal.Copy(IntPtr.Add(new IntPtr(ptr), offset), arr, 0, num);
        this._view.SafeMemoryMappedViewHandle.ReleasePointer();
        return arr;
    }

    public unsafe void WriteBytes(int offset, byte[] data)
    {
        byte* ptr = (byte*)0;
        this._view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
        Marshal.Copy(data, 0, IntPtr.Add(new IntPtr(ptr), offset), data.Length);
        this._view.SafeMemoryMappedViewHandle.ReleasePointer();
    }



         */

        public byte[] MmfToArray(MemoryMappedViewAccessor mmfa)
        {
            //Создаем обьект нужного размера
            byte[] array = new byte[mmfa.Capacity];

            //Читаем оперативку и пишем в массив
            mmfa.ReadArray(0, array, 0, array.Length);

            //Возвращаем массив с файлом
            return array;

        }

        //Копируем файл в оперативку и получаем указатель
        public MemoryMappedViewAccessor MoveToMmf(string pathToFile, string MMF_Name="")
        {
                    
            //Создаем обьект в оперативке из файла
            MemoryMappedFile mmFile = null;

            //Если название обьекта пусто            
            if (MMF_Name == "")
            {
                mmFile = MemoryMappedFile.CreateFromFile(pathToFile, System.IO.FileMode.Open);
            }
            else
            {
                mmFile = MemoryMappedFile.CreateFromFile(pathToFile, System.IO.FileMode.Open, MMF_Name);
            }

            //Получаем  указатель на файл в оперативке                   
            return mmFile.CreateViewAccessor(); 
        }
    }
}
