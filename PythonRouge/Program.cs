// CSharpRouge Copyright (C) 2017 Tom Pinnock
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
//  details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see
// http://www.gnu.org/licenses/.

using PythonRouge.game;
using PythonRouge.Network;
using RLNET;
using System;
using System.IO;

namespace PythonRouge
{
    public static class Program
    {
        //Initialize variables for engines and controlling rootConsole.
        private static RLRootConsole _rootConsole;
        private static SPEngine _spEngine;
        private static MpEngine _mpEngine;
        public static bool Multi;
        public static bool main = true;
        public static bool opts = false;
        private static bool multiRendered = false;
        private static bool detsEntered = false;
        private static bool wantName;
        private static string name = "";
        private static MPLobby _lobby;

        public static string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        /// <summary>
        ///     Main entry point for the program
        ///     This is where the program starts here the various settings are set
        ///     and the root console is created we also hook the events handlers
        ///     for each event. Then we start the console.
        /// </summary>
        public static void Main()
        {
            var fontImage = @"iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAIAAABMXPacAAAAB3RJTUUH3wEZFiMyURjM3AAAABd0RVh0U29mdHdhcmUAR0xEUE5HIHZlciAzLjRxhaThAAAACHRwTkdHTEQzAAAAAEqAKR8AAAAEZ0FNQQAAsY8L 
                            / GEFAAAABmJLR0QA / wCAAMBJdqXDAAAJvElEQVR4nO1X0ZIbSQib///puUpdbjMGSQi6xzt7MQ+pNg1CEm0nOQ4eZwonfz0ENDECXglugiQD9Ou1ZI1WjghnJcxnX6py2DsEfErMOEaDwR7JsqwrnFlNuC0EhqL
                            zdbd5nukCFK/1wytWWRoHrWSsBA6UJgyByCqEeD8EG5PlKa3JjkCvr4rydJjMebEA6BIzLSSLBTDXmGBonGbARpRQexfgYIpZIqnlY/ev7JlrEIjhCvb5iuW1vPwReqp1wREs6TBk9HJXDFhx8tCNJaCZ9ynl22uGI
                            cAaCM6sFFDn67Oo1TmqTJsc/DJvktRdol63M5K67JrPBb8Ovn0nWjXL5DM0qOV1qbCVf0o44nMNdA0uwDnk9pIJ3D1EaC2mu62rXg11krD+xgu3ACWNNH1nc5naFh9oROmRrmFXEEqT/H2Ls9U/ByEzRggiw4MTrNj
                            Md/VqWBOKtYCsHqMF52HwVhwU0f5L1/Vsrq4p9eahTEJkCWfAmlKYpguh4Lh867u5Uq/JBD75ULa8jDDtC5UlNFMCe4XLAifTa+V/fKwLYxaXxa3Gnx1QZHjLZZ7hlHNbIOtD3xkWt/JHwFwAfLOs18H39egH1BqxM
                            dwprQWcKMQw4cKBvgED98VtKe2+6I1oVeuX6z9SZ38+SUFGhzPUr3eoLjU4GpjXMOMLay0gs/WhFuNW8JcB10liJbmMJf3RTl6MaOG0+Azdb7VBE01CcGctArc+5y0L2MLkb4w3/XSUvxKw3smHWziUQTGS7Mrn042
                            V76LiA8WEBobunNlsoQdedec6fMzRmq0pgfLRjuvBJt2vj3BWa6IWc9MCRBJKKIXgBbTs0FMZgiNbq4IgrL6EcmJ7b0wGis4CSk7C0BJQL0xn/J21YtbuWhdMKRfgmOUYMcMZLMBPtvJnCgcHBwRiA8RgxobdavZdN
                            wUOI+Pn9dUdIH9jfJs77EVDTuIbcFTP/CRfl+sty2t8yJaRhLcMVivNH1k+DK2BzNmtc0nOz0PM0qYQpXYBcnWGnS0fcmcpzDGX6WS9sJ4NdcB1l9Yl9IZkiIPbSDHHC7gWa6msLIOIPMOHLpRdpa4SJM89kePQz8N
                            fQOkFG8AEsF5Yr4fmuY53or4267+PmaTgU2Ay4xyi3bMjrEvpjgWsaHEwI6vzEiYb2AKhWHHGuZLLyAyN8RRQmg80S881F5CnfOIBAdebr7bXr5wP48VpHEGJAWrr4C3sokCmgG59uH3IArQW4Y95KxZTQCwugI3MV
                            7sW4HAOZ8aqxd+R5u7gfA1TfHZc7GDjAqA2OBqSNCk59TDvL7WADmuEoKKx1DZbQAg9VOgPyLpYo1kW69AsTe+YcU6GucksDreQg5DGRjtWmiMawVzOBeKjhtKGihaRYaaXfPLckn82pFX/iUcEXPyfzflrHO+81eg
                            X60oNAp//IvNhtAaM2bQ8HdQvxniReyLPYL9x5UsxZYzxD/R4Bb4pGWIySqz4fA3Iv71LYZwgMavX7DU+u2rpgl3aTUbA9IcGNI5hMVDHaMe1cgHlleZ28N1DnNsX0BXcHSxIsKustkXVL9ZduXjFBxVXqeyQBTCuj
                            J8JwuwIxSb+XhzoD6vR0p4b52vsgtpF7wfET1HLdsOes0Z4ypqfwqMKwdPZypF+i2ALhmA/c2GTTr2TXzlDPiKfz5Anc1lciXoBElsg11L8SRZjsh8vQNAzcUpRrfAbqVezBbAzZLNrASGEC4I/zOg8u/J3Juj1fjq
                            cqUJwafRBFikWcAXv8jcXw652LsBshpIOrlZwvRYrfikJ3XcaM09WXHotlMLIc3/3atecgmdG+SAeFDOiuQVuOL8+1p7BB/zHc1l9xvdxyo9qsCAkXNAZP/zevXNLfBPcXE8DEaKzAeIFOTjd/Pa5QrJT/5W0vL0Wr
                            ZwFrE6aeXMWFO/zX1cUMnkcwIEXmjTDglesvpUvrQktjKfDX0wpkxn/lA+CYrEGk2I5uHs++AsyrSn5++pEktkIt1LMYD0CBbY4OE6+lC34l8W+LieZP2aB1uCnxU/hOYmWNvhCdy1yC47zwMVjZ5RgGTOB8V9VV36
                            t/LPWv1IPC1rq9uqlaGeKUApjpu39oTmXemGLANf4NFn6mEEF0YzWyjtmtQwNZ7O3RV74cxg7a8yAw5553rWA7pXgQNHggCeY2DL6K7KKGc55WVhugcUQH07BFEtCcJiet5hn+LkLaCNJgZDr9dAy8/UxH/4noVcyB
                            hwXs/0d6b1iLPZYWhp86vpl+XzWSXY5bBw9DO3FSX43fOTD+DpvDHNWuY+czPJjS/e5bQlBaLCAdZ6MjzJuO1RrveJpsP3nem2o7vJJCj6Zc7gSBzid+aBumdoGxPTM5vrBeh3MgbrMdtUif2MT9AVJeTciqc8shJW
                            aKusSUIqbIzij63wgvW6WCMZnAOKcnQV8Yin0Rm9xPKz0eoB/hgfuvIhWHn6BZrogThd/gOMyh9YflftHWkM5+LOAYkA+6x3ksx4My3J7KQxWMhq+cSeKdRzB/A+EOMM/T7SGzAk6Zeb9BZT48LbEN/MCx2f+uzqf9
                            Q7yWVBkZTqfFeauLg4zbnFui881/8O+ASy/C0fg7wqMDNegd5DPasBnAYwntP6o3D/SGuiAUf4vWkB50aWyq37XYvTQ99d/FvDN9T9sAV8RivUCQj2LsoDNNcPHr41YN7RbD/NB2AxkELtw6hnjuBsnGAHzTr3JYTu
                            OqFlamJ/fghOE+eCL9XfPncRnARvnTuKzgI1zt21M4Gx5CwP8lmU5nPouJuj9LOBxC+gSYjiMTZk38UueDuxAb7emwM/CZiFwGHJr4n3437YAp8gCIiMZG53fxSqPa7kzBveXVAfs/0quok//x5u7ukwg8mcBBf5nA
                            TH5WcDtC1gMn+4d+GyoIDPjydbJqDZAGXS4Qi8jPlVzNmthsAy5LGYeGd5QwiUZUACFaRf0YIfQUe2SYYbigFOaUnJjGcYz5LVYMIIJgyPFACFSZ3RxCcWKsztwXCk5NzJANtFdwL9nU9iBwhFZFpfjTGFOmdOrjRZ5
                            pjdyy/3XfEZkA9aFBRwmRteXfLo8Mx9mEWsv8eu/A7R+Rxj0CLqmRzjycp4NFYay6SZPNtcRWKPfGt25YsG3xptm7Xqh16twOxhhIo8jrJNtd89Q/YLYgG7+ehVq/AVkUzTyOBjDLW+lR4ItBg7WVFoLKNt9gwbh4+u
                            HaPHRRusBft5vKRevVbQoabT80X86Xf4ulXKwuF036Cqp21XmBbjOC4sskmcKR9UgTFWwONffwXOwXQ21hMMW080HwPzRXMB74g0P8duCPeqnSX0zq38AkpsLsVLOAuMAAAAASUVORK5CYII=";
            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(fontImage));
            MemoryStream bitMapStream = new MemoryStream(bitmapData);
            FileStream bitFile = new FileStream("%TEMP%\rlFontFile.png", FileMode.Create);
            bitMapStream.WriteTo(bitFile);
            var settings = new RLSettings();
            settings.BitmapFile = fontImage;
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = 90;
            settings.Height = 70;
            settings.Scale = 1f;
            settings.Title = "PythonRouge";
            settings.WindowBorder = RLWindowBorder.Fixed;
            settings.ResizeType = RLResizeType.ResizeCells;
            settings.StartWindowState = RLWindowState.Normal;

            _rootConsole = new RLRootConsole(settings);

            _rootConsole.Update += rootConsole_Update;
            _rootConsole.Render += rootConsole_Render;
            _rootConsole.Run();
        }


        /// <summary>
        ///     This is the event handler for the Root Console update event.
        ///     Here the keypresses are handled and first a check for either mpengine or spengine meaning
        ///     all keys are sent to the respective engine
        ///     If no engine is running we can traverse the menus using keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            
           if(Menu.currMenu == "main")
            {
                switch(Menu.mainUpdate(_rootConsole))
                {
                    case 1:
                        _spEngine = new SPEngine(_rootConsole);
                        break;
                    case 2:
                        Menu.currMenu = "multi";
                        break;
                }
            }
           if(Menu.currMenu == "multi")
            {
                switch(Menu.multiUpdate(_rootConsole))
                {
                    case 1:
                        break;
                    case 2:
                        Menu.currMenu = "enterDets";
                        Menu.name = "";
                        break;
                }
            }
           if(Menu.currMenu == "enterDets")
            {
                switch(Menu.enterDetsUpdate(_rootConsole))
                {
                    case 1:
                        _lobby = new MPLobby(_rootConsole, Menu.name);
                        Menu.currMenu = "game";
                        break;
                    case 0:
                        break;
                }
            }
            if (Menu.currMenu == "game")
            {
                var keypress = _rootConsole.Keyboard.GetKeyPress();
                if (keypress != null)
                {
                    _spEngine?.handleKey(keypress);
                    _mpEngine?.HandleKey(keypress);
                }
                var m = _lobby?.Update(keypress);
                if(m == 1)
                {
                    _mpEngine = new MpEngine(_rootConsole, Menu.name, _lobby?.servers[_lobby.sellist[_lobby.curIndex]]);
                    _lobby = null;
                }
                _mpEngine?.Update();
            }

        }

        /// <summary>
        ///     This is the render event handler which is where we handle the respective render methods,
        ///     that need to be running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            _rootConsole.Clear();
            switch (Menu.currMenu)
            {
                case "main":
                    Menu.mainRender(_rootConsole);
                    break;
                case "multi":
                    Menu.multiRender(_rootConsole);
                    break;
                case "enterDets":
                    Menu.enterDetsRender(_rootConsole);
                    break;
                case "game":
                    _spEngine?.render();
                    _mpEngine?.Render();
                    _lobby?.Render();
                    break;


            }
            
            
            _rootConsole.Draw();
        }
    }
}