﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsAssetPipeline.Audio.Wwise
{
    public static class WwiseEnums
    {
        public enum ParameterTypes
        {
            Volume = 0x0,
            LFE = 0x1,
            Pitch = 0x2,
            LPF = 0x3,
            HPF = 0x4,
            BusVolume = 0x5,
            InitialDelay = 0x6,
            MakeUpGain = 0x7,
            Deprecated_FeedbackVolume = 0x8,
            Deprecated_FeedbackLowpass = 0x9,
            Deprecated_FeedbackPitch = 0xA,
            MidiTransposition = 0xB,
            MidiVelocityOffset = 0xC,
            PlaybackSpeed = 0xD,
            MuteRatio = 0xE,
            PlayMechanismSpecialTransitionsValue = 0xF,
            MaxNumInstances = 0x10,
            Priority = 0x11,
            Position_PAN_X_2D = 0x12,
            Position_PAN_Y_2D = 0x13,
            Position_PAN_X_3D = 0x14,
            Position_PAN_Y_3D = 0x15,
            Position_PAN_Z_3D = 0x16,
            PositioningTypeBlend = 0x17,
            Positioning_Divergence_Center_PCT = 0x18,
            Positioning_Cone_Attenuation_ON_OFF = 0x19,
            Positioning_Cone_Attenuation = 0x1A,
            Positioning_Cone_LPF = 0x1B,
            Positioning_Cone_HPF = 0x1C,
            BypassFX0 = 0x1D,
            BypassFX1 = 0x1E,
            BypassFX2 = 0x1F,
            BypassFX3 = 0x20,
            BypassAllFX = 0x21,
            HDRBusThreshold = 0x22,
            HDRBusReleaseTime = 0x23,
            HDRBusRatio = 0x24,
            HDRActiveRange = 0x25,
            GameAuxSendVolume = 0x26,
            UserAuxSendVolume0 = 0x27,
            UserAuxSendVolume1 = 0x28,
            UserAuxSendVolume2 = 0x29,
            UserAuxSendVolume3 = 0x2A,
            OutputBusVolume = 0x2B,
            OutputBusHPF = 0x2C,
            OutputBusLPF = 0x2D,
            Positioning_EnableAttenuation = 0x2E,
            ReflectionsVolume = 0x2F,
            UserAuxSendLPF0 = 0x30,
            UserAuxSendLPF1 = 0x31,
            UserAuxSendLPF2 = 0x32,
            UserAuxSendLPF3 = 0x33,
            UserAuxSendHPF0 = 0x34,
            UserAuxSendHPF1 = 0x35,
            UserAuxSendHPF2 = 0x36,
            UserAuxSendHPF3 = 0x37,
            GameAuxSendLPF = 0x38,
            GameAuxSendHPF = 0x39,
            Position_PAN_Z_2D = 0x3A,
            BypassAllMetadata = 0x3B,
            MaxNumRTPC = 0x3C,
        }

        public enum PropTypes
        {
            Volume = 0x00,
            LFE = 0x01,
            Pitch = 0x02,
            LPF = 0x03,
            HPF = 0x04,
            BusVolume = 0x05,
            MakeUpGain = 0x06,
            Priority = 0x07,
            PriorityDistanceOffset = 0x08,
            REMOVED_FeedbackVolume = 0x09,
            REMOVED_FeedbackLPF = 0x0A,
            MuteRatio = 0x0B,
            PAN_LR = 0x0C,
            PAN_FR = 0x0D,
            CenterPCT = 0x0E,
            DelayTime = 0x0F,
            TransitionTime = 0x10,
            Probability = 0x11,
            DialogueMode = 0x12,
            UserAuxSendVolume0 = 0x13,
            UserAuxSendVolume1 = 0x14,
            UserAuxSendVolume2 = 0x15,
            UserAuxSendVolume3 = 0x16,
            GameAuxSendVolume = 0x17,
            OutputBusVolume = 0x18,
            OutputBusHPF = 0x19,
            OutputBusLPF = 0x1A,
            HDRBusThreshold = 0x1B,
            HDRBusRatio = 0x1C,
            HDRBusReleaseTime = 0x1D,
            HDRBusGameParam = 0x1E,
            HDRBusGameParamMin = 0x1F,
            HDRBusGameParamMax = 0x20,
            HDRActiveRange = 0x21,
            LoopStart = 0x22,
            LoopEnd = 0x23,
            TrimInTime = 0x24,
            TrimOutTime = 0x25,
            FadeInTime = 0x26,
            FadeOutTime = 0x27,
            FadeInCurve = 0x28,
            FadeOutCurve = 0x29,
            LoopCrossfadeDuration = 0x2A,
            CrossfadeUpCurve = 0x2B,
            CrossfadeDownCurve = 0x2C,
            MidiTrackingRootNote = 0x2D,
            MidiPlayOnNoteType = 0x2E,
            MidiTransposition = 0x2F,
            MidiVelocityOffset = 0x30,
            MidiKeyRangeMin = 0x31,
            MidiKeyRangeMax = 0x32,
            MidiVelocityRangeMin = 0x33,
            MidiVelocityRangeMax = 0x34,
            MidiChannelMask = 0x35,
            PlaybackSpeed = 0x36,
            MidiTempoSource = 0x37,
            MidiTargetNode = 0x38,
            AttachedPluginFXID = 0x39,
            Loop = 0x3A,
            InitialDelay = 0x3B,
            UserAuxSendLPF0 = 0x3C,
            UserAuxSendLPF1 = 0x3D,
            UserAuxSendLPF2 = 0x3E,
            UserAuxSendLPF3 = 0x3F,
            UserAuxSendHPF0 = 0x40,
            UserAuxSendHPF1 = 0x41,
            UserAuxSendHPF2 = 0x42,
            UserAuxSendHPF3 = 0x43,
            GameAuxSendLPF = 0x44,
            GameAuxSendHPF = 0x45,
            AttenuationID = 0x46,
            PositioningTypeBlend = 0x47,
            ReflectionBusVolume = 0x48,
            PAN_UD = 0x49,
        }
    }
}