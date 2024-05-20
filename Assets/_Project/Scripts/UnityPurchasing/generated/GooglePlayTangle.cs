// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("kCKhgpCtpqmKJugmV62hoaGloKPHFW+9cxaGCwRj+FNN85gk55BFDmCuIPurcnAojNYBOoiqUsUYCK9nHPHmJ+vRelE3H62y+8zFZCQMSip6TEr7NO4QiRCkiuUmxljhrMrn975+EJI2UcchPD2Sm2HmZpV/TsiWkmNk3dC13Nj3pexHza29jynoci0Avsv4THdQQ2ZYihOgP9PbLNzAjiKhr6CQIqGqoiKhoaArGjLtaIOBWBlhFUjI5sEHUDzpTYzUtExxQF06hRFG6VEgfTibc6W2t0kBOnRsT6PklVOg6LlrGwUU+lQjWiucqetBWxR0ZcH9ElyINYDxPtRjSGWzDrxB4wLP/CyuhPMt4OUJNXqoc6rqfB3PRSZ6cv2G0aKjoaCh");
        private static int[] order = new int[] { 0,5,3,13,13,9,8,11,9,12,11,11,13,13,14 };
        private static int key = 160;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
