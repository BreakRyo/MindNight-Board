using System.Collections.Generic;

public class SystemBlock{
    public char messageType;
    public bool passed = false;
    public int proposer;
    public int[] propose;
    public int[] accepted;
    public int[] refused;
    public bool waitForOptional = false;
    public int hacks;
    
}