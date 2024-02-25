using UnityEngine;

public class AirWindSimulation : MonoBehaviour
{
    public Vector3 windDirection = Vector3.forward;
    public float windStrength = 5f;
    public float dragCoefficient = 0.1f;
    public float liftCoefficient = 0.1f;

    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Apply wind force to particles
        ApplyWind();

        // Visualize wind direction (optional)
        VisualizeWindDirection();
    }

    private void ApplyWind()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            // Calculate relative velocity
            Vector3 relativeVelocity = particles[i].velocity - windDirection * windStrength;

            // Calculate aerodynamic forces (lift and drag)
            Vector3 liftForce = Vector3.Cross(particles[i].velocity, -transform.right).normalized * relativeVelocity.magnitude * liftCoefficient;
            Vector3 dragForce = -relativeVelocity.normalized * relativeVelocity.sqrMagnitude * dragCoefficient;

            // Apply aerodynamic forces to particles
            particles[i].velocity += (liftForce + dragForce) * Time.deltaTime;
        }

        particleSystem.SetParticles(particles, numParticlesAlive);
    }

    private void VisualizeWindDirection()
    {
        Debug.DrawRay(transform.position, windDirection.normalized * 2f, Color.blue);
    }
}
