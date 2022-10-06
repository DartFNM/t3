using System;
using System.Collections.Generic;
using Neo.IronLua;


namespace T3.Core.DataTypes
{
    public class LuaVM : IDisposable
    {
        public Lua Lua = new Lua();
        public LuaGlobal Env;
        public LuaResult LastResult = null;
        public bool IsInitInvoked = false;
        public double PrevUpdateTime = double.NaN;
        public bool IsValid;


        public SortedSet<string> Libraries = new();

        public LuaVM()
        {
            Lua = new Lua();
            Env = Lua.CreateEnvironment();
            IsValid = true;
        }

        public void Dispose()
        {
            IsValid = false;
            Env = null;
            Lua.Dispose();
            Lua = null;
        }

        public bool IsLibraryDeclared(string name)
        {
            return Libraries.Contains(name);
        }


    }; // class
}