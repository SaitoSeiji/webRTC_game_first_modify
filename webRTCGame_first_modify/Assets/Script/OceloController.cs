using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceloController
{
    Ban<Koma_ocelo> _myBan;

    public OceloController()
    {
        _myBan = new Ban<Koma_ocelo>(new Vector2Int(8, 8));
    }


}
