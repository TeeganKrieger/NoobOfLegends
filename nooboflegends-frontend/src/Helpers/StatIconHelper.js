import React from 'react';

import goldIcon from '../Resources/StatIcons/Gold.png';
import killsIcon from '../Resources/StatIcons/Kills.png';
import deathsIcon from '../Resources/StatIcons/Deaths.png';
import assistsIcon from '../Resources/StatIcons/Assists.png';
import timeSpentDeadIcon from '../Resources/StatIcons/TimeSpentDead.png';
import totalDamageIcon from '../Resources/StatIcons/TotalDamageDealt.png';
import baronKillsIcon from '../Resources/StatIcons/BaronKills.png';
import dragonKillsIcon from '../Resources/StatIcons/DragonKills.png';
import minionKillsIcon from '../Resources/StatIcons/MinionKills.png';
import jungleMinionKillsIcon from '../Resources/StatIcons/JungleMinionKills.png';
import visionScoreIcon from '../Resources/StatIcons/VisionScore.png';
import killParticipationIcon from '../Resources/StatIcons/KillParticipation.png';
import healingIcon from '../Resources/StatIcons/Healing.png';



export default function GetStatIcon(stat) {
        switch (stat) {
            case "gold":
                return goldIcon;
            case "kills":
                return killsIcon;
            case "deaths":
                return deathsIcon;
            case "assists":
                return assistsIcon;
            case "timeSpentDead":
                return timeSpentDeadIcon;
            case "totalDamageDealt":
                return totalDamageIcon;
            case "baronKills":
                return baronKillsIcon;
            case "dragonKills":
                return dragonKillsIcon;
            case "minionKills":
                return minionKillsIcon;
            case "jungleMinionKills":
                return jungleMinionKillsIcon;
            case "visionScore":
                return visionScoreIcon;
            case "killParticipation":
                return killParticipationIcon;
            case "healing":
                return healingIcon;
        }
    }
