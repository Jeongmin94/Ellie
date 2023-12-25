using Channels.Components;
using Data.Monster;
using Monsters.AbstractClass;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder
{
    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context
    {
        public NavMeshAgent agent;
        public Animator animator;
        public MonsterAudioController audioController;
        public AudioSource audioSource;
        public BoxCollider boxCollider;

        public BehaviourTreeController btController;
        public CapsuleCollider capsuleCollider;
        public CharacterController characterController;

        public AbstractMonster controller;
        public GameObject gameObject;
        public MonsterParticleController particleController;
        public Rigidbody physics;
        public SphereCollider sphereCollider;
        public TicketMachine ticketMachine;
        public Transform transform;

        public static Context CreateFromGameObject(GameObject gameObject)
        {
            // Fetch all commonly used components
            var context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.GetComponentInChildren<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.sphereCollider = gameObject.GetComponent<SphereCollider>();
            context.boxCollider = gameObject.GetComponent<BoxCollider>();
            context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            context.characterController = gameObject.GetComponent<CharacterController>();

            // Add whatever else you need here...
            context.controller = gameObject.GetComponent<AbstractMonster>();
            context.audioController = gameObject.GetComponent<MonsterAudioController>();
            context.particleController = gameObject.GetComponent<MonsterParticleController>();
            context.audioSource = gameObject.GetComponent<AudioSource>();

            context.btController = gameObject.GetComponent<BehaviourTreeController>();
            context.ticketMachine = gameObject.GetComponent<TicketMachine>();

            return context;
        }
    }
}