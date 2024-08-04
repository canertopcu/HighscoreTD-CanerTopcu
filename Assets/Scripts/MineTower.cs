using UnityEngine;

namespace Assets.Scripts
{
    public class MineTower : Tower
    {
        public int pathIndex = -1;


        [ContextMenu("Explode")]
        public void OnExplode()
        {
            if (pathIndex != -1)
            {
                if (gameData.activeMineSlots.Contains(pathIndex))
                {
                    gameData.activeMineSlots.Remove(pathIndex);
                }
                var vfx = Instantiate(vfxData.mineExplosion, transform.position, Quaternion.identity);
                GetComponent<MeshRenderer>().enabled = false;
                Destroy(vfx, 2f);
                Destroy(gameObject, 3f);

            }
        }
    }

}
