// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("AbMwEwE8Nzgbt3m3xjwwMDA0MTLxP7FqOuPhuR1HkKsZO8NUiZk+9o1gd7Z6QOvApo48I2pdVPW1ndu7L++BA6fAVrCtrAMK8Hf3BO7fWQfKheX0UGyDzRmkEWCvRfLZ9CKfLZEvWmnd5sHS98kbgjGuQkq9TVEfVoT+LOKHF5qV8mnC3GIJtXYB1J+zMD4xAbMwOzOzMDAxuoujfPkSEMmI8ITZWXdQlsGteNwdRSXd4NHM0HKTXm29PxVivHF0mKTrOeI7e+2rFIDXeMCx7KkK4jQnJtiQq+X93jJ1BMIxeSj6ipSFa8Wyy7oNOHrQA/L1TEEkTUlmNH3WXDwsHrh547zr3dtqpX+BGIE1G3S3V8lwPVt2Zoxe1Lfr42wXQDMyMDEw");
        private static int[] order = new int[] { 0,3,13,9,4,11,11,9,12,11,11,11,12,13,14 };
        private static int key = 49;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
