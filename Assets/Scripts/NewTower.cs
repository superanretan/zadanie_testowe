using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview
{
    public class NewTower : MonoBehaviour
    {

        private IReadOnlyList<Enemy> enemies;

        public void Initialize(IReadOnlyList<Enemy> enemies)
        {
            this.enemies = enemies;
           // fireTimer = firingRate;
        }
        
        
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
