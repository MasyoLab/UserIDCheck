using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace UserIDCheck {

    class MainClass {

        private const int FIRST_INDEX = 1;
        private const string STR_BEGIN = "(ID[";
        private const string STR_END = "])";

        static void Main() {
            var main = new MainClass();

            var folders = main.GetFolder();
            if (!folders.Any()) {
                Console.WriteLine("実行ファイルにフォルダをドロップしてください。");
                Console.ReadKey();
                return;
            }

            var subFolders = Directory.GetDirectories(folders[0]);
            var userIdDict = new Dictionary<string, int>(subFolders.Length);
            var errorData = new List<string>();

            foreach (var item in subFolders) {
                if (!item.Contains(STR_BEGIN)) {
                    errorData.Add(item);
                    continue;
                }

                var str = item.Substring(0, item.IndexOf(STR_BEGIN));
                str = item.Replace(str, string.Empty);
                str = str.Replace(STR_BEGIN, string.Empty);
                str = str.Replace(STR_END, string.Empty);

                if (!userIdDict.ContainsKey(str)) {
                    userIdDict.Add(str, 0);
                }
                userIdDict[str]++;
            }

            foreach (var valuePair in userIdDict.OrderBy(o => o.Value)) {
                Console.WriteLine($"{valuePair.Value}:{valuePair.Key}");
            }

            foreach (var str in errorData) {
                Console.WriteLine(str);
            }

            Console.ReadKey();
        }

        public IReadOnlyList<string> GetFolder() {
            // コマンドライン引数を配列で取得する
            string[] files = Environment.GetCommandLineArgs();

            var strs = new List<string>();
            if (files.Length > FIRST_INDEX) {
                for (int i = FIRST_INDEX; i < files.Length; i++) {
                    strs.Add(files[i]);
                }
            }
            return strs;
        }
    }
}
