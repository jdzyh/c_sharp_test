using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace FlightGame {
    /// <summary> 
    /// ************************************************ 
    /// 
    /// 飞行棋游戏 v1.0 
    ///  
    /// 编写：五子连星（http://stwzlx.blog.51cto.com） 
    /// 
    /// ************************************************ 
    /// </summary> 
    internal class Program {
        //数组的下标为0的元素对应地图上的第1格，下标为1的元素对应第2格…下标为n的元素对应第n+1格 

        /// <summary> 
        /// Map数组用来存放地图的格数 
        /// </summary> 
        private static int[] Map = new int[100];

        /// <summary> 
        /// playerPos数组存放玩家的坐标 
        /// </summary> 
        private static int[] playerPos;

        /// <summary> 
        /// playerIco数组用来存放玩家的标识字符（不能小于玩家人数） 
        /// </summary> 
        private static string[] playerIco = { "Ａ", "Ｂ", "Ｃ", "Ｄ" };

        /// <summary> 
        /// playerNum变量用来存放玩家的人数 
        /// </summary> 
        private static int playerNum = 0;

        private static void Main(string[] args) {
            Random r = new Random();    //产生一个随机数 
            int step = 0;   //用于存放临时产生的随机数 

            string msg = "";    //设置msg变量用来接收玩家掷骰子后的信息 

            bool win = false;   //设置win变量来存放是否有玩家获胜 

            ShowUI();

            playerNum = PlayerNum();    //调用PlayerNum()方法获得用户输入的人数 

            playerPos = new int[playerNum];     //根据玩家人数产生初始坐标 

            string[] name = PlayerName(playerNum);  //调用PlayerName()方法得到每一个玩家的昵称 

            bool[] isStop = new bool[playerNum];  //根据玩家人数产生一个数组，存放玩家是否处于暂停状态 

            Console.Clear();

            ShowUI();

            //初始化地图数据 
            InitialMap();

            //绘制地图 
            DrawMap();

            Console.WriteLine("开始游戏……");

            while (win == false)    //判断是否有人胜出 
            {
                for (int i = 0; i < playerNum; i++)     //根据玩家人数循环游戏 
                {
                    if (isStop[i] == true)      //当前玩家是否暂停一次 
                    {
                        isStop[i] = false;      //恢复暂停标记为未暂停 
                        Console.WriteLine("{0}暂停一次！", name[i]);
                        Console.WriteLine("*****************************");
                        continue;  //当前玩家暂停一次，继续循环 
                    } else {
                        Console.WriteLine("{0}按任意键开始掷骰子……", name[i]);

                        //取得用户按下的键 
                        ConsoleKeyInfo rec = Console.ReadKey(true);    //Console.ReadKey(true)不显示用户按下的键 

                        //作弊键 
                        if (rec.Key == ConsoleKey.F12 && rec.Modifiers == ConsoleModifiers.Control)  //按下 Ctrl + F12 键 
                        {
                            step = Num(1, 100);
                        } else {
                            step = r.Next(1, 7);    //得到一个1到6之间的随机数 
                        }

                        Console.WriteLine("{0}掷出了：{1}", name[i], step);

                        Console.WriteLine("按任意键开始行动……");

                        Console.ReadKey(true);

                        playerPos[i] += step;   //玩家向前走了掷出点数的格数 
                        CheckPos(); //判断玩家走动后的格数是否超界 

                        bool encounter = false;     //用来存储是否有玩家被踩到 

                        //判断当前玩家是否踩到某一位玩家 
                        for (int j = 0; j < playerNum; j++) {
                            if (i != j && playerPos[i] == playerPos[j]) {
                                playerPos[j] = 0;       //被踩到的玩家退回到起点 
                                encounter = true;       //标志有玩家被踩到 
                                msg = string.Format("{0}踩到了{1},{1}退回起点！", name[i], name[j]);
                                break;
                            }
                        }

                        //没人被踩到 
                        if (encounter == false) {
                            switch (Map[playerPos[i]])  //当前玩家位置 
                            {
                                case 1:     //幸运轮盘 
                                    Console.Clear();
                                    ShowUI();
                                    DrawMap();

                                    //获得前一个玩家的编号 
                                    int ex;
                                    if (i == 0) {
                                        ex = playerNum - 1;
                                    } else {
                                        ex = i - 1;
                                    }

                                    Console.WriteLine("请选择运气（1：和{0}交换位置；2：轰炸{0}）：", name[ex]);

                                    int userSelect = Num(1, 2);     //让用户选择使用哪种运气 

                                    if (userSelect == 1)    //和前一位玩家交换位置 
                                    {
                                        int temp = playerPos[i];
                                        playerPos[i] = playerPos[ex];
                                        playerPos[ex] = temp;

                                        msg = string.Format("{0}和{1}交换了位置", name[i], name[ex]);
                                    } else   //轰炸前一位玩家，使其退6格 
                                    {
                                        playerPos[ex] = playerPos[ex] - 6;
                                        CheckPos();
                                        msg = string.Format("{0}轰炸{1}，{1}退六格", name[i], name[ex]);
                                    }

                                    break;

                                case 2:     //地雷 
                                    playerPos[i] = playerPos[i] - 6;    //当前玩家退后六格 
                                    CheckPos();
                                    msg = string.Format("{0}踩到了\"地雷\"退六格", name[i]);
                                    break;

                                case 3:     //暂停 
                                    isStop[i] = true;   //设置当前玩家暂停标记 
                                    break;

                                case 4:     //时空隧道 
                                    playerPos[i] = playerPos[i] + 10;   //当前玩家前进10格 
                                    CheckPos();
                                    msg = string.Format("{0}走到了\"时空隧道\"前进十格", name[i]);
                                    break;

                                default:
                                    msg = "";
                                    break;
                            }
                        }
                    }

                    Console.Clear();
                    ShowUI();
                    DrawMap();

                    //显示玩家遇到关卡等信息 
                    if (msg != "") {
                        Console.WriteLine(msg);
                        Console.WriteLine("*****************************");
                    }

                    if (playerPos[i] == 99)   //如果当前玩家走到终点，则游戏结束 
                    {
                        Console.Clear();
                        ShowUI();
                        DrawMap();
                        Console.WriteLine("{0}最先走到终点，获得胜利！！！！", name[i]);
                        win = true;
                        Console.ReadKey();
                        break;
                    }
                }
            }

            Console.ReadKey();
        }

        /// <summary> 
        /// 游戏名称及规则等介绍 
        /// </summary> 
        private static void ShowUI() {
            Console.SetWindowSize(80, 50);

            Console.WriteLine("*********************************************************************");
            Console.WriteLine("*                     骑    士    飞    行    棋                    *");
            Console.WriteLine("*         游戏制作：五子连星（http://stwzlx.blog.51cto.com）        *");
            Console.WriteLine("*                                                                   *");
            Console.WriteLine("*最多4名玩家，轮流掷骰子，有一位先达到终点结束游戏。                *");
            Console.WriteLine("*被踩到的玩家退回起点                                               *");
            Console.WriteLine("*走到“幸运轮盘”可以选择和前一位玩家交换位置或者轰炸他（使其退6格）*");
            Console.WriteLine("*走到“地雷”需要退后6格                                            *");
            Console.WriteLine("*走到“暂停”需要暂停掷骰子一次                                     *");
            Console.WriteLine("*走到“时空隧道”可以往前移动10格                                   *");
            Console.WriteLine("*********************************************************************");
        }

        /// <summary> 
        /// 得到用户输入的一个minValue和maxValue之间的整数 
        /// </summary> 
        /// <param name="minValue">最小值</param> 
        /// <param name="maxValue">最大值</param> 
        /// <returns></returns> 
        private static int Num(int minValue, int maxValue) {
            while (true) {
                try {
                    int number = Convert.ToInt32(Console.ReadLine());
                    if (number < minValue || number > maxValue) {
                        Console.WriteLine("必须输入一个{0}到{1}之间的整数！请重新输入：", minValue, maxValue);

                        continue;
                    }

                    return number;
                } catch {
                    Console.WriteLine("只能输入一个整数！请重新输入：");
                    continue;
                }
            }
        }

        /// <summary> 
        /// 获得参加游戏的人数。 
        /// 最小人数为1，playerMax控制最大人数。 
        /// </summary> 
        /// <returns>参加的人数</returns> 
        private static int PlayerNum() {
            int playerMax = 4;  //最大参加人数 

            Console.WriteLine("请输入参加的人数(1—{0}人）：", playerMax);

            return Num(1, playerMax);    //返回游戏人数 
        }

        /// <summary> 
        /// 让用户输入每一个玩家的姓名，并返回所有玩家的姓名。 
        /// </summary> 
        /// <param name="number">玩家的人数</param> 
        /// <returns>所有玩家的昵称</returns> 
        private static string[] PlayerName(int number) {
            //定义一个数组存放玩家姓名 
            string[] name = new string[number];

            for (int i = 0; i < name.Length; i++) {
                Console.WriteLine("请输入第{0}位玩家的姓名：", i + 1);
                name[i] = Console.ReadLine();
                while (name[i] == "")   //判断玩家姓名是否为空，为空则重新输入 
                {
                    Console.WriteLine("姓名不能为空，请重新输入第{0}位玩家姓名：", i + 1);
                    name[i] = Console.ReadLine();
                }
                if (i > 0)  //判断玩家人数，两位以上需要核对姓名是否相同 
                {
                    for (int j = 0; j < i; j++)     //当前输入的玩家姓名与已经存在的所有玩家姓名进行对照，看是否相同 
                    {
                        if (name[i] == name[j]) {
                            Console.WriteLine("该姓名与第{0}位玩家相同，请重新输入第{1}位玩家姓名：", j + 1, i + 1);
                            name[i] = Console.ReadLine();
                        }
                    }
                }
            }
            return (string[])name;      //返回玩家姓名 
        }

        /// <summary> 
        /// 设置地图关卡的位置 
        /// </summary> 
        private static void InitialMap() {
            //在下面的数组存储我们游戏地图各个关卡 

            //在数组中用以下数字表示相关图标 
            //1：幸运轮盘—◎ 
            //2：地雷—★ 
            //3：暂停—▲ 
            //4：时空隧道—※ 
            //0：普通—□ 

            //定义相关关卡的位置 
            int[] luckyTurn = { 6, 23, 40, 55, 69, 83 };      //幸运轮盘1 
            int[] landMine = { 5, 13, 17, 33, 38, 50, 64, 80, 94 };      //地雷2 
            int[] pause = { 9, 27, 60, 93 };        //暂停3 
            int[] timeTunel = { 20, 25, 45, 63, 72, 88, 90 };      //时空隧道4 

            for (int i = 0; i < luckyTurn.Length; i++) {
                Map[luckyTurn[i]] = 1;  //把地图Map的第luckyTurn[i]格设置为1(幸运轮盘) 
            }

            for (int i = 0; i < landMine.Length; i++) {
                Map[landMine[i]] = 2;   //把地图Map的第landMine[i]格设置为2(地雷) 
            }

            for (int i = 0; i < pause.Length; i++)  //把地图Map的第pause[i]格设置为3(暂停) 
            {
                Map[pause[i]] = 3;
            }

            for (int i = 0; i < timeTunel.Length; i++)  //把地图Map的第timeTunel[i]格设置为4(时空隧道) 
            {
                Map[timeTunel[i]] = 4;
            }
        }

        /// <summary> 
        /// 判断当前格应该绘制的图标 
        /// </summary> 
        /// <param name="pos">当前格位置</param> 
        /// <returns>当前格图标</returns> 
        private static string MapIco(int pos) {
            string ico = "";     //存放要绘制的图标字符 

            //判断是否有两个以上的玩家处于当前格上 
            int j = 0;
            for (int i = 0; i < playerNum; i++) {
                if (playerPos[i] == pos)     //判断玩家是否在当前格上 
                {
                    j++;
                    if (j >= 2)     //有两个以上玩家在同一个格上 
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;  //设置图标颜色 
                        ico = ("<>");
                        return ico;
                    }
                }
            }

            //判断当前格上是否有某一位玩家存在 
            for (int i = 0; i < playerNum; i++) {
                if (playerPos[i] == pos)    //当前格有玩家playerPos[i]存在 
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;  //设置图标颜色 
                    ico = playerIco[i];     //获得该玩家的标识字符 
                    return ico;
                }
            }

            switch (Map[pos])     //根据当前格的值来显示相应的图标 
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Magenta; //设置图标颜色 
                    ico = ("◎");     //1：幸运轮盘—◎ 
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;    //设置图标颜色 
                    ico = ("★");     //2：地雷—★ 
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Cyan;    //设置图标颜色 
                    ico = ("▲");     //3：暂停—▲ 
                    break;

                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;   //设置图标颜色 
                    ico = ("※");     //4：时空隧道—※ 
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.White;   //设置图标颜色 
                    ico = ("□");     //0：普通—□ 
                    break;
            }
            return ico;
        }

        /// <summary> 
        /// 绘制地图 
        /// </summary> 
        private static void DrawMap() {
            //图例及玩家说明文字 
            Console.WriteLine("图例说明：幸运轮盘—◎   地雷—★   暂停—▲   时空隧道—※   普通—□");
            Console.Write("玩家说明：多名玩家—<>  ");
            for (int i = 0; i < playerNum; i++) {
                Console.Write("第{0}位玩家:{1}  ", i + 1, playerIco[i]);
            }
            Console.WriteLine();    //新起一行 

            //画第一行 
            for (int i = 0; i <= 29; i++) {
                Console.Write(MapIco(i));       //绘制当前格的图标 
            }

            Console.WriteLine();    //第一行结束，换行 

            //画第右边列（包含5行，每行前29格为空字符） 
            for (int i = 30; i <= 34; i++)      //循环绘制5行 
            {
                for (int j = 0; j < 29; j++)    //有图标的字符串需要绘制在第30格，因此需要每行前29格绘制两个空字符串 
                {
                    Console.Write("  ");
                }
                Console.WriteLine(MapIco(i)); ;      //绘制当前格的图标，并换行 
            }

            //画第二行 
            for (int i = 64; i >= 35; i--) {
                Console.Write(MapIco(i));     //绘制当前格的图标 
            }

            Console.WriteLine();    //第二行结束，换行 

            //画第左边列 
            for (int i = 65; i <= 69; i++) {
                Console.WriteLine(MapIco(i));      //绘制当前格的图标，并换行 
            }

            //画第三行 
            for (int i = 70; i <= 99; i++) {
                Console.Write(MapIco(i));       //绘制当前格的图标 
            }

            Console.WriteLine();

            Console.ResetColor();   //重置控制台的前景色为默认 
        }

        /// <summary> 
        /// 对玩家的坐标是否越界进行判断 
        /// </summary> 
        private static void CheckPos() {
            for (int i = 0; i < playerNum; i++) {
                if (playerPos[i] > 99)      //如果坐标超过99格则设置为到99格 
                {
                    playerPos[i] = 99;
                } else if (playerPos[i] <= 0) //如果坐标超过0格则设置为到0格 
                {
                    playerPos[i] = 0;
                }
            }
        }
    }
} 
