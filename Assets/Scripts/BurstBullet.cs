
using UnityEngine;

namespace AFSInterview
{
  public class BurstBullet : MonoBehaviour
  {
    private void OnCollisionEnter(Collision collision)
    {
      if (collision.collider.GetComponent<Enemy>() != null)
      {
        Destroy(collision.collider.gameObject);
        Destroy(this.gameObject);
      }
      else if (collision.collider.CompareTag("Ground") || collision.collider.GetComponent<BurstTower>() != null ||
               collision.collider.GetComponent<SimpleTower>() != null)
      {
        Destroy(this.gameObject);
      }
    }
  }
}
