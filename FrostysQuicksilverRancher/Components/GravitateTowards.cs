using UnityEngine;

namespace FrostysQuicksilverRancher.Components
{
	class GravitateTowards : SRBehaviour
	{
		Vacuumable vac;
		Rigidbody rb;
		GameObject target;
		float gravitateSpeed = 10f;

		public void Awake()
		{
			vac = GetComponent<Vacuumable>();
			rb = GetComponent<Rigidbody>();
		}

		public void SetTarget(GameObject target)
		{
			this.target = target;
		}

		public void FixedUpdate()
		{
			if (vac && !vac.isCaptive() && target)
			{
				rb.AddForce((target.transform.position - transform.position).normalized * gravitateSpeed * rb.mass);
			}
		}

		
	}
}
