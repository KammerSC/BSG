using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player
{   //id - a játékos id-ja, chartype  - a karakter száma, pos - ahol a karakter áll
    public byte id, chartype, pos;  
    public bool revealed, sympatizer, admiral, president, brig;
    public byte[] skillset;
    public List<LoyaltyCard> loyalties = new List<LoyaltyCard>();
    public List<SkillCard> skillcards = new List<SkillCard>();

    public Player(){}

    public Player(byte id, byte chartype)
    {
        this.id = id;
        this.chartype = chartype;
    }

    public void SetupFromData(byte data)
    {

    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Client id: " + id + ", ");
        switch (chartype)
        {
            case 0: sb.Append("Name: William Adam"); break;
            case 1: sb.Append("Name: Saul Tigh"); break;
            case 2: sb.Append("Name: Karl \"Helo\" Agaton"); break;
            case 3: sb.Append("Name: Laura Rosline"); break;
            case 4: sb.Append("Name: Gaius Baltar"); break;
            case 5: sb.Append("Name: Tom Zarek"); break;
            case 6: sb.Append("Name: Lee “Apollo” Adama"); break;
            case 7: sb.Append("Name: Kara “Starbuck” Thrace"); break;
            case 8: sb.Append("Name: Sharon “Boomer” Valerii"); break;
            case 9: sb.Append("Name: Galen Tyrol"); break;
            case 10: sb.Append("Name: ANY"); break;
        }
        if (admiral)
            sb.Append(", <ADMIRAL>");
        if (president)
            sb.Append(", <PRESIDENT>");

        for (int i = 0; i < loyalties.Count; i++)
            sb.Append(", " + loyalties[i]);

        return sb.ToString();
    }

}
