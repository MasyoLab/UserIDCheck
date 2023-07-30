using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace UserIDCheck
{
    struct AppConstData
    {
        public const int FIRST_INDEX = 1;
        public const string STR_BEGIN = "(ID[";
        public const string STR_END = "])";
    }

    class LoadFolder
    {
        public IReadOnlyList<string> Folders = new List<string>();

        private static IReadOnlyList<string> GetCommandLineArgs()
        {
            // コマンドライン引数を配列で取得する
            string[] files = Environment.GetCommandLineArgs();

            var strs = new List<string>();
            if (files.Length > AppConstData.FIRST_INDEX)
            {
                for (int i = AppConstData.FIRST_INDEX; i < files.Length; i++)
                {
                    strs.Add(files[i]);
                }
            }
            return strs;
        }

        public bool Execute()
        {
            var folders = GetCommandLineArgs();
            if (!folders.Any())
            {
                Console.WriteLine("実行ファイルにフォルダをドロップしてください。");
                Console.ReadKey();
                return false;
            }

            Folders = Directory.GetDirectories(folders[0]);
            return Folders.Any();
        }
    }

    class CheckUserDuplicate
    {
        private IReadOnlyList<string> _folders;

        public CheckUserDuplicate(IReadOnlyList<string> folders)
        {
            _folders = folders;
        }

        public void Execute()
        {
            var userIdDict = new Dictionary<string, int>(_folders.Count);
            var errorData = new List<string>();

            foreach (var item in _folders)
            {
                if (!item.Contains(AppConstData.STR_BEGIN))
                {
                    errorData.Add(item);
                    continue;
                }

                var str = item.Substring(0, item.IndexOf(AppConstData.STR_BEGIN));
                str = item.Replace(str, string.Empty);
                str = str.Replace(AppConstData.STR_BEGIN, string.Empty);
                str = str.Replace(AppConstData.STR_END, string.Empty);

                if (!userIdDict.ContainsKey(str))
                {
                    userIdDict.Add(str, 0);
                }
                userIdDict[str]++;
            }

            foreach (var valuePair in userIdDict.OrderBy(o => o.Value))
            {
                Console.WriteLine($"{valuePair.Value}:{valuePair.Key}");
            }

            foreach (var str in errorData)
            {
                Console.WriteLine(str);
            }
        }
    }

    class MainClass
    {
        static void Main()
        {
            var loadFIle = new LoadFolder();
            if (!loadFIle.Execute())
            {
                return;
            }

            var checkUserDuplicate = new CheckUserDuplicate(loadFIle.Folders);
            checkUserDuplicate.Execute();

            Console.ReadKey();
        }
    }
}
