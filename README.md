# Attention-Driven Scenography (ADS)

PLEASE NOTE: Documentation update in progress, missing some elements.

ADS is a Unity plugin formalising the design framework from my PhD thesis. It allows tracking of Current and Cumulative Attention to drive dynamic and long-term changes in environmental storytelling in games and other game engine-based experiences. 

For a working example of an experience using ADS, please see [*Woolgatherer*](https://hardcore-ascetic.itch.io/woolgatherer)

# Plugin Contents

The plugin contains two main folders: Scripts and TestScene.

Scripts contains the main ADS components for driving attention-based interactions.

TestScene contains assets and a scene showing some potential interaction implementations.

# ADS Structure and Usage

ADS consists of the following key scripts/components:

* AttentionTracker
* AttentionBehaviour
  * AttentionBehaviourExample
* AttentionComparisonBehaviour
  * AttentionComparisonBehaviourExample
* AttentionDatastore

## AttentionTracker

Any GameObject can track attention on it by adding an AttentionTracker component to it. AttentionTrackers provide two kinds of tracking:

* Current Attention (moment to moment)
* Cumulative Attention (over time)

Current Attention is based on where an object is in the primary camera view. Offscreen, an object has a Current Attention value of 0.0. At the exact centre of the screen, an object has a Current Attention value of 1.0. Current Attention ranges between these two values, and can be read by an AttentionBehaviour continuously, or compared to thresholds for specific states, to drive Current Attention-based interactions.

Cumulative Attention is a scaled value based on the total amount of Current Attention accumulated on a given object. For example, if an object were in the centre of the screen for five seconds, having a Current Attention value of 1.0, it would have a Cumulative Attention value of 5.0.

## AttentionBehaviour

## AttentionBehaviourExample

## AttentionComparisonBehaviour

## AttentionComparisonBehaviourExample

## Attention Datastore (not functioning as intended at present)

# Test Scene Explainer

# Further Scripts
