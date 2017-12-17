using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo
{
    public sealed class SHA512Encrypt
    {
        public static string SHA512En(string password, string salt)
        {
            /*
            用户密码加密算法

            1、创建 SHA-512 加密算法 hasher
            2、使用 salt 和 password（原始密码） 调用 hasher.update
            3、获取加密后的值 hv
            4、重复 512 次调用 hasher.update(hv)，每次hv都更新为最新的 hasher.digest 加密值
            5、最终的 hv 值做 base64 编码，保存为 password
             * 密码明文：：123456
               salt：i6gcdkl48494b9tfb5x67xs7yht4szv4f43e3jnzl6ckrfcl 
            */
           password = salt + password;
           // password = "i6gcdkl48494b9tfb5x67xs7yht4szv4f43e3jnzl6ckrfcl" + password;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result;
            System.Security.Cryptography.SHA512 shaM = new System.Security.Cryptography.SHA512Managed();
            result = shaM.ComputeHash(bytes);
            int i = 0;
            while (i++ < 512)
            {
                result = shaM.ComputeHash(result);
            }
            shaM.Clear();
            return Convert.ToBase64String(result);
        }
    }
}
