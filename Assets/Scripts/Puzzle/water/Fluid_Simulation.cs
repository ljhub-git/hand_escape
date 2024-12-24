using UnityEngine;

public class Fluid_Simulation : MonoBehaviour
{
    public float gravity = 0f;  
    public float flowSpeed = 2.0f;  
    private Vector2 position;
    private Vector2 velocity;



    // Update is called once per frame
    void Update()
    {

        velocity += Vector2.down * gravity * Time.deltaTime;

        position += velocity * Time.deltaTime;

        Flow();

        DrawCircle(position, 1);
    }

    void Flow()
    {
        position += new Vector2(flowSpeed * Time.deltaTime, 0);
    }

    // 원을 생성하는 함수 (프리팹 없이)
    void DrawCircle(Vector2 position, float radius)
    {
        // 3D Sphere 오브젝트를 만들어 2D 원처럼 사용
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // 원의 위치를 설정
        sphere.transform.position = new Vector3(position.x, position.y, 0);  // Z 값은 0으로 설정하여 2D처럼 보이도록 함

        // 크기를 2D 원처럼 보이게 조정 (Z축 크기를 1로 설정)
        sphere.transform.localScale = new Vector3(radius, radius, radius);  // X, Y 축만 크기 조정, Z는 1로 유지

        // 물리적 상호작용을 위해 Rigidbody 추가
        Rigidbody rb = sphere.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = sphere.AddComponent<Rigidbody>();  // Rigidbody 없으면 추가
        }
        rb.linearVelocity = velocity;  // 물리적 속도 적용

        // Collider 설정
        SphereCollider collider = sphere.GetComponent<SphereCollider>();
        if (collider == null)
        {
            collider = sphere.AddComponent<SphereCollider>();  // SphereCollider 없으면 추가
        }
    }
}
