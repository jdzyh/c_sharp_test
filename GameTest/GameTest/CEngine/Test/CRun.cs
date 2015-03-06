using CEngine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRun
{
    class CRun {
        static void Main(string[] args) {
            CGameEngine.Run(new TestGame());  
        } 
    }
}
