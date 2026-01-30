using UnityEngine;
using System.Collections.Generic;

namespace MaskGame
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance;

        private class ParticleData
        {
            public GameObject obj;
            public Vector3 velocity;
            public float lifeTime;
            public Renderer rend;
        }

        private List<ParticleData> particles = new List<ParticleData>();
        private List<ParticleData> pool = new List<ParticleData>();

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                ParticleData p = particles[i];
                p.lifeTime -= Time.deltaTime;

                if (p.lifeTime <= 0)
                {
                    p.obj.SetActive(false);
                    pool.Add(p);
                    particles.RemoveAt(i);
                }
                else
                {
                    p.obj.transform.position += p.velocity * Time.deltaTime;
                    p.velocity += Vector3.down * 9.8f * Time.deltaTime; // Gravity
                    p.obj.transform.Rotate(Vector3.one * 100f * Time.deltaTime);
                    p.obj.transform.localScale = Vector3.Lerp(p.obj.transform.localScale, Vector3.zero, Time.deltaTime * 2f);
                }
            }
        }

        public void SpawnBreakEffect(Vector3 position, Color color)
        {
            for (int i = 0; i < 8; i++)
            {
                SpawnParticle(position, color, 0.5f);
            }
        }

        public void SpawnJumpEffect(Vector3 position)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnParticle(position, Color.white, 0.3f);
            }
        }

        private void SpawnParticle(Vector3 pos, Color col, float scale)
        {
            ParticleData p;
            if (pool.Count > 0)
            {
                p = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
                p.obj.SetActive(true);
            }
            else
            {
                p = new ParticleData();
                p.obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(p.obj.GetComponent<Collider>());
                p.rend = p.obj.GetComponent<Renderer>();
                p.obj.transform.parent = transform;
            }

            p.obj.transform.position = pos + Random.insideUnitSphere * 0.2f;
            p.obj.transform.localScale = Vector3.one * scale;
            p.rend.material.color = col;

            p.velocity = Random.insideUnitSphere * 5f;
            p.velocity.y = Mathf.Abs(p.velocity.y) + 2f; // Upward bias
            p.lifeTime = 1.0f;

            particles.Add(p);
        }
    }
}
