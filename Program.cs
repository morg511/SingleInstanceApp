using System;
using System.Threading;

class Program
{
    static void Main()
    {
        bool createdNew = false;
        Mutex? mutex = null;

        try
        {
            mutex = new Mutex(
                true,
                "SingleInstanceApp_Mutex",
                out createdNew
            );

            if (!createdNew)
            {
                Console.WriteLine("Программа уже запущена!");
                Console.WriteLine("Второй экземпляр запрещен.");
                return;
            }

            Console.WriteLine("Программа запущена успешно!");
            Console.WriteLine("Повторный запуск запрещен.");

            long before = GC.GetTotalMemory(false);
            byte[] data = new byte[1024 * 1024];
            long after = GC.GetTotalMemory(false);

            Console.WriteLine();
            Console.WriteLine("Память до создания массива: " +
                              before / 1024 + " КБ");
            Console.WriteLine("Память после создания массива: " +
                              after / 1024 + " КБ");

            GC.KeepAlive(data);

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для выхода.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        finally
        {
            if (createdNew && mutex != null)
            {
                mutex.ReleaseMutex();
            }

            mutex?.Dispose();
        }
    }
}