using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator2D : MonoBehaviour
{
    public class Branch 
    {
        
        public Vector2 _start;
        public Vector2 _end;
        public Vector2 _direction;
        public Branch _parent;

        public int _verticesId; // branch index
        public List<Branch> _children = new List<Branch>();

        // Fractal AI
        public List<Vector2> _attractors = new List<Vector2>();

        // render properties
        public int _distanceFromRoot = 0;
        public bool _grown = false;

        public Branch(Vector2 start, Vector2 end, Vector2 direction, Branch parent = null) {
			_start = start;
			_end = end;
			_direction = direction;
			_parent = parent;
		}
    }

    public int _nbAttractors = 50;

    // the attractor points
    public List<Vector2> _attractors = new List<Vector2>();

    // a list of the active attractors 
	public List<int> _activeAttractors = new List<int>();

    // reference to the first branch 
	Branch _firstBranch;

    // the branches 
	List<Branch> _branches = new List<Branch>();

    // a list of the current extremities 
	public List<Branch> _extremities = new List<Branch>();

    public float _attractionRange = 0.1f;

    public float _killRange = 0.2f;

    public float _timeBetweenIterations = 0.1f;

	// the elpsed time since the last iteration, this is used for the purpose of animation
	float _timeSinceLastIteration = 0f;

    void GenerateAttractors (int n, float r) {
		for (int i = 0; i < n; i++) {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);

            Vector2 pt = new Vector2(x - 0.5f, y - 0.5f);

			pt += new Vector2(transform.position.x, transform.position.y);

			_attractors.Add(pt);
		}
	}


    void Awake () {
		// initilization
	}



    public Vector2 _startPosition = new Vector2(0, 0);
	[Range(0f, 0.5f)]
	public float _branchLength = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateAttractors(_nbAttractors, 4);

        // we generate the first branch 
		_firstBranch = new Branch(_startPosition, _startPosition + new Vector2(0, _branchLength), new Vector2(0, 1));
		_branches.Add(_firstBranch);
		_extremities.Add(_firstBranch);
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastIteration+= Time.deltaTime;

		// we check if we need to run a new iteration 
		if (_timeSinceLastIteration > _timeBetweenIterations) {
			_timeSinceLastIteration = 0f;


            // we parse the extremities to set them as grown 
			foreach (Branch b in _extremities) {
				b._grown = true;
			}


			// we remove the attractors in kill range
			for (int i = _attractors.Count-1; i >= 0; i--) {
				foreach (Branch b in _branches) {
					if (Vector2.Distance(b._end, _attractors[i]) < _killRange) {
						_attractors.Remove(_attractors[i]);
						_nbAttractors--;
						break;
					}
				}
			}


            if (_attractors.Count > 0) {
				// we clear the active attractors
				_activeAttractors.Clear();
				foreach (Branch b in _branches) {
					b._attractors.Clear();
				}


                // each attractor is associated to its closest branch, if in attraction range
				int ia = 0;
				foreach (Vector2 attractor in _attractors) {
					float min = 999999f;
					Branch closest = null; // will store the closest branch
					foreach (Branch b in _branches) {
						float d = Vector2.Distance(b._end, attractor);
						if (d < _attractionRange && d < min) {
							min = d;
							closest = b;
						}
					}

					// if a branch has been found, we add the attractor to the branch
					if (closest != null) {
						closest._attractors.Add(attractor);
						_activeAttractors.Add(ia);
					}

					ia++;
				}



				// if at least an attraction point has been found, we want our tree to grow towards it
				if (_activeAttractors.Count != 0) {
					// because new extremities will be set here, we clear the current ones
					_extremities.Clear();

					// new branches will be added here
					List<Branch> newBranches = new List<Branch>();

					foreach (Branch b in _branches) {
				// 		// if the branch has attraction points, we grow towards them
						if (b._attractors.Count > 0) {
				// 			// we compute the direction of the new branch
							Vector2 dir = new Vector2(0, 0);
							foreach (Vector2 attr in b._attractors) {
								dir += (attr - b._end).normalized;
							}
							dir/= b._attractors.Count;
				// 			// random growth
				// 			// dir+= RandomGrowthVector();
							dir.Normalize();

				// 			// our new branch grows in the correct direction
							Branch nb = new Branch(b._end, b._end + dir * _branchLength, dir, b);
							nb._distanceFromRoot = b._distanceFromRoot+1;
							b._children.Add(nb);
							newBranches.Add(nb);
							_extremities.Add(nb);
						} else {
				// 			// if no attraction points, we only check if the branch is an extremity
							if (b._children.Count == 0) {
								_extremities.Add(b);
							}
						}
					}

				// 	// we merge the new branches with the previous ones
					_branches.AddRange(newBranches);
                } else {
				// 	// we grow the extremities of the tree
					for (int i = 0; i < _extremities.Count; i++) {
						Branch e = _extremities[i];
						// the new branch starts where the extremity ends
						Vector2 start = e._end;
						// we add randomness to the direction
						Vector2 dir = e._direction;
						// we add the direction multiplied by the branch length to get the end point
						Vector2 end = e._end + dir * _branchLength;
						// a new branch can be created with the same direction as its parent
						Branch nb = new Branch(start, end, dir, e);

						// the current extrimity has a new child
						e._children.Add(nb);

						// let's add the new branch to the list and set it as the new extremity 
						_branches.Add(nb);
						_extremities[i] = nb;
					}
                }
            }
        }
    }

    void OnDrawGizmos () {
		
		if (_attractors == null) {
			return;
		}
		if (_activeAttractors == null) {
			return;
		}
		if (_extremities == null) {
			return;
		}
		// we draw the attractors
		for (int i = 0; i < _attractors.Count; i++) {
            if (_activeAttractors.Contains(i)) {
				Gizmos.color = Color.yellow;
			} else {
				Gizmos.color = Color.red;
			}
			Gizmos.DrawSphere( new Vector3(_attractors[i].x, _attractors[i].y, 0), 0.03f);
		}


        Gizmos.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
		// Gizmos.DrawSphere(_extremities[0]._end, _attractionRange);
		
		// we draw the branches 
		foreach (Branch b in _branches) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(b._start, b._end);
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(b._end, 0.02f);
			Gizmos.DrawSphere(b._start, 0.02f);
		}
	}
}
