using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags", menuName = "Scriptable Objects/Flags", order = 0)]
public class Flags : ScriptableObject {
    [Serializable]
    public class Flag{
        public string FlagName;
        public bool IsSet;
    }
    
    public Flag[] FlagList;

    public void SetFlag(string name, bool b){
        foreach (Flag f in FlagList)
        {
            if(f.FlagName.Equals(name)){
                f.IsSet = b;
                return;
            }
        }
    }

    // toggles the flag
    public void SetFlag(string name){
        foreach (Flag f in FlagList)
        {
            if(f.FlagName.Equals(name)){
                f.IsSet = !f.IsSet;
                return;
            }
        }
    }

    // checks a flag's state
    // 1 - Flag exists and is True
    // 0 - Flag exists and is False
    // -1 - Flag does not exist
    public int CheckFlag(string name){
        foreach (Flag f in FlagList)
        {
            if(f.FlagName.Equals(name)){
                return f.IsSet ? 1 : 0;
            }
        }

        return -1;
    }
}