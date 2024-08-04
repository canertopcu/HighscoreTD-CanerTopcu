using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewVFX", menuName = "VFX/New VFX")]
    public class VfxDataSO : ScriptableObject
    {
        public GameObject mineExplosion;
    }
}
