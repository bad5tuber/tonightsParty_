    using UnityEngine;

    public class LightOscillator : MonoBehaviour
    {
        Quaternion startingRot;
        [SerializeField] Quaternion movementQuaternion;
        [SerializeField] [Range(-359,359)] float angleModifier = 0;
        [SerializeField] float period = 2f;
        public float movementFactor = 2f;

        public Quaternion movementFactorQuaternion;

        // Start is called before the first frame update
        void Start()
        {
            startingRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (period <= Mathf.Epsilon) {return;}
            
            float cycles = Time.time / period; // continually growing over time

            const float tau = Mathf.PI * 2; // constant value of 6.283xxx
            float rawSinWave = Mathf.Sin(cycles * tau); // going from 01 to 1

            
            // movementfactor will be cycling between -1 and 1, so we add 1 to get to
            // 0 -> 2 then divide by 2 to get to 0 -> 1 which can be used with our
            // Serialized Field

            movementFactor = ((rawSinWave +1f) * angleModifier);

            movementFactorQuaternion = Quaternion.Euler(movementFactor, movementFactor, 0f);
            
            
            Quaternion offset = movementQuaternion * movementFactorQuaternion;
            transform.rotation = startingRot * offset;

        }
    }
