using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace c4dnbInstaller.Tools
{
    class Helper
    {
        //------//搜索字符串(参数1：完整的内容，参数2：左边的内容，参数3：右边的内容)----(获取两个字符串中间的字符串)       
        public static string StringClamp(string s, string s1, string s2)  //获取搜索到的数目  
        {
            int n1, n2;
            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置  
            n2 = s.IndexOf(s2, n1);               //结束位置    
            return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回    
        }

        //拷贝目录
        public static void DirectoryCopy(string sourcePath, string targetPath)
        {
            DirectoryImprovePermissions(Directory.GetParent(targetPath).ToString());

            Directory.CreateDirectory(targetPath);
            //创建所有新目录
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                DirectoryInfo dir = new DirectoryInfo(targetPath);

                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //复制所有文件 & 保持文件名和路径一致
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                string _targetPath = newPath.Replace(sourcePath, targetPath);
                File.Copy(newPath, _targetPath, true);
            }

        }

        //判断程序是否以管理员身份运行
        public static bool IsAdministrator()
        {
            bool result;
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                result = principal.IsInRole(WindowsBuiltInRole.Administrator);

                //http://www.cnblogs.com/Interkey/p/RunAsAdmin.html
                //AppDomain domain = Thread.GetDomain();
                //domain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                //WindowsPrincipal windowsPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
                //result = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 为文件添加users，everyone用户组的完全控制权限
        /// </summary>
        /// <param name="filePath"></param>
        public static void FileImprovePermissions(string filePath)
        {

            //获取文件信息
            FileInfo fileInfo = new FileInfo(filePath);
            //获得该文件的访问权限
            System.Security.AccessControl.FileSecurity fileSecurity = fileInfo.GetAccessControl();
            //添加ereryone用户组的访问权限规则 完全控制权限
            fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
            //添加Users用户组的访问权限规则 完全控制权限
            fileSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //设置访问权限
            fileInfo.SetAccessControl(fileSecurity);
        }

        /// <summary>
        ///为文件夹添加users，everyone用户组的完全控制权限
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DirectoryImprovePermissions(string dirPath)
        {
            //获取文件夹信息
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            //获得该文件夹的所有访问权限
            System.Security.AccessControl.DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            //设定文件ACL继承
            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //添加ereryone用户组的访问权限规则 完全控制权限
            FileSystemAccessRule everyoneFileSystemAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //添加Users用户组的访问权限规则 完全控制权限
            FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            bool isModified = false;
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            //设置访问权限
            dir.SetAccessControl(dirSecurity);
        }

        //打开指定网址
        public static void LinkOpen(string link)
        {
            Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }
    }
}
