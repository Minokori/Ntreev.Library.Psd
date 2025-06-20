namespace Ntreev.Library.Psd;

partial class PsdReader
    {
    private static void ValidateValue<T>(T value, string name, Func<T> readFunc)
        {
        T v = readFunc();
        if (object.Equals(value, v) == false)
            {
            throw new InvalidFormatException($"The value of {name} is not {value}.");
            }
        }

    private static bool ReverseValue(bool value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToBoolean(bytes, 0);
        }

    private static double ReverseValue(double value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
        }

    private static short ReverseValue(short value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToInt16(bytes, 0);
        }

    private static int ReverseValue(int value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToInt32(bytes, 0);
        }

    private static long ReverseValue(long value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToInt64(bytes, 0);
        }

    private static ushort ReverseValue(ushort value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
        }

    private static uint ReverseValue(uint value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
        }

    private static ulong ReverseValue(ulong value)
        {
        byte[] bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        return BitConverter.ToUInt64(bytes, 0);
        }
    /// <summary>
    /// return TRUE if <see cref="ReadType"/> returns "8BIM" or FALSE for "8B64"
    /// </summary>
    /// <param name="check64bit"></param>
    /// <returns></returns>
    private bool VerifySignature(bool check64bit)
        {
        string signature = this.ReadType();

        if (signature == "8BIM")
            return true;

        if (check64bit == true && signature == "8B64")
            return true;

        return false;
        }

    private void Skip(int count)
        {
        this.ReadBytes(count);
        }

    }

