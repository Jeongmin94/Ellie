using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Assets.Scripts.Monsters;
using Channels.Components;

namespace TheKiwiCoder {
    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public SphereCollider sphereCollider;
        public BoxCollider boxCollider;
        public CapsuleCollider capsuleCollider;
        public CharacterController characterController;

        public MonsterController controller;
        public MonsterAudioController audioController;
        public MonsterParticleController particleController;
        public AudioSource audioSource;

        public BehaviourTreeController btController;
        public TicketMachine ticketMachine;

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
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
            context.controller = gameObject.GetComponent<MonsterController>();
            context.audioController = gameObject.GetComponent<MonsterAudioController>();
            context.particleController = gameObject.GetComponent<MonsterParticleController>();
            context.audioSource = gameObject.GetComponent<AudioSource>();

            context.btController = gameObject.GetComponent<BehaviourTreeController>();
            context.ticketMachine = gameObject.GetComponent<TicketMachine>();

            return context;
        }
    }
}