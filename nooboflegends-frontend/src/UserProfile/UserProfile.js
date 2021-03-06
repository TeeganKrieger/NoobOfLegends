import React, { Component } from 'react';
import UserInfo from './UserInfo'
import MatchList from './MatchList'
import StatSelector from './StatSelector'
import StatChart from './StatChart'
import GetColorSet from '../Helpers/DistinctColorGenerator'
import SkillDisplay from './SkillDisplay';
import RankedHelper from '../Helpers/RankedIconHelper'
import './UserProfile.css';

/** Component that renders an entire user profile using various sub components. */
export default class UserProfile extends Component {

    constructor(props) {
        super(props);

        this.state = {
            allMatches: [], userInfo: this.GetDefaultUserInfo(props.additionalProps.searchFor), selectedMatches: [], activeStat: { id: "gold", name: "Gold", lambda: (m) => m.gold }, skills: [],
            changePage: props.changePageFunc
        };
    }

    componentDidMount() {
        this.fetchCheckpointId(this.props.additionalProps.searchFor);
    }

    componentDidUpdate(prevProps) {
        if (this.props.additionalProps.searchFor != prevProps.additionalProps.searchFor) {
            this.setState({ allMatches: [], userInfo: this.GetDefaultUserInfo(this.props.additionalProps.searchFor), selectedMatches: [], skills: [] });
            this.fetchCheckpointId(this.props.additionalProps.searchFor);
        }
    }

    /** An array containing all tracked stats, their ids, names and lambda expressions. */
    allStats = [
        { id: "gold", name: "Gold", lambda: (m) => m.gold },
        { id: "kills", name: "Kills", lambda: (m) => m.kills },
        { id: "deaths", name: "Deaths", lambda: (m) => m.deaths },
        { id: "assists", name: "Assists", lambda: (m) => m.assists },
        { id: "timeSpentDead", name: "Time Spent Dead", lambda: (m) => m.timeSpentDead },
        { id: "totalDamageDealt", name: "Total Damage Dealt", lambda: (m) => m.totalDamageDealt },
        { id: "baronKills", name: "Baron Kills", lambda: (m) => m.baronKills },
        { id: "dragonKills", name: "Dragon Kills", lambda: (m) => m.dragonKills },
        { id: "minionKills", name: "Minion Kills", lambda: (m) => m.minionKills },
        { id: "jungleMinionKills", name: "Jungle Minion Kills", lambda: (m) => m.jungleMinionKills },
        { id: "visionScore", name: "Vision Score", lambda: (m) => m.visionScore },
        { id: "killParticipation", name: "Kill Participation", lambda: (m) => m.killParticipation },
        { id: "healing", name: "Healing", lambda: (m) => m.healing },
    ]

    /**
     * A lambda expression passed to StatSelector components to facilitate changing the selected stat.
     * @param {any} stat The id of the stat to select.
     */
    selectStatFunc = (stat) => {
        this.setState({ activeStat: stat });
    }

    /**
     * A lambda expression passed to MatchListing components to facilitate selecting a match.
     * @param {any} match The match to select.
     */
    selectMatch = (match) => {
        let selected = this.state.selectedMatches ?? [];
        this.setState({ selectedMatches: [] }); //Little fix for react error when modifying arrays within state
        selected.push(match);
        this.setState({ selectedMatches: selected });
    }

    /**
     * A lambda expression passed to MatchListing components to facilitate unselecting a match.
     * @param {any} match The match to deselect.
     */
    deselectMatch = (match) => {
        let matches = this.state.selectedMatches ?? [];
        let newMatches = matches.filter((m) => m.id != match.id);
        this.setState({ selectedMatches: newMatches });
    }

    render() {

        var statSelectors = [];

        for (let i = 0; i < this.allStats.length; i++) {
            statSelectors.push(<StatSelector key={i} stat={this.allStats[i]} selectStatFunc={this.selectStatFunc} />)
        }

        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div id='user-profile-parent' className='col-12 col-md-7 col-xl-5'>
                        <UserInfo user={this.state.userInfo} />
                        <MatchList matches={this.state.allMatches} select={this.selectMatch} deselect={this.deselectMatch} />
                    </div>
                    <div className='d-none d-xl-block col-xl-1'>
                    </div>
                    <div className='col-12 col-md-1'>
                        {statSelectors}
                    </div>
                    <div className='col-12 col-md-4'>
                        <SkillDisplay skills={this.state.skills} />
                        <StatChart stat={this.state.activeStat} matches={this.state.selectedMatches} />
                        <button className="analyze" onClick={this.fetchSkills.bind(this)}>Analyze</button>
                    </div>
                </div>
            </div>
        );
    }

    //Fetch
    /**
     * Initiates a profile task on the backend and gets a checkpoint id.
     * @param {any} usernameAndTagline The username and tagline of the player to search for.
     */
    async fetchCheckpointId(usernameAndTagline) {

        let split = usernameAndTagline.split("#");

        if (split.length != 2) {
            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };

            this.state.changePage("Home", props, "LoadingPage (31)");
            return;
        }

        //Make Request to backend for user. If User is found, redirect to other page
        const checkpointResponse = await fetch("api/user/start/" + split[0] + "/" + split[1], {
            method: 'GET',
            mode: 'no-cors',
        });

        if (checkpointResponse.status == 200) {

            const checkpointId = await checkpointResponse.text();

            console.log("Starting Loop");
            this.getCheckpointUpdate(checkpointId);

        } else if (checkpointResponse.status == 400) {

            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };

            this.state.changePage("Home", props, "UserProfile");

        } else {

            const props = {
                "error": "Server"
            };

            this.state.changePage("Home", props, "UserProfile");
        }
    }

    /**
     * Uses a checkpoint id to recieve occasional updates from the backend to populate the page.
     * @param {any} checkpointId The ID of the checkpoint.
     */
    async getCheckpointUpdate(checkpointId) {

        //Make Request to backend for user. If User is found, redirect to other page
        const checkpointResponse = await fetch("api/user/check/" + checkpointId, {
            method: 'GET',
            mode: 'no-cors',
        });

        if (checkpointResponse.status == 200) {
            const checkpoint = await checkpointResponse.json();

            if (checkpoint.error != 0) {
                const props = {
                    "error": "BadRequest",
                    "usernameAndTagline": ""
                };

                this.state.changePage("Home", props, "UserProfile");
            }

            let userInfo = this.state.userInfo;
            if (checkpoint.userInfo != null)
                userInfo = checkpoint.userInfo;


            let currentMatches = this.state.allMatches;
            currentMatches = currentMatches.concat(checkpoint.matches);

            let colorSeed = this.state.userInfo.username ?? "Default";
            let colors = GetColorSet(colorSeed, 100);

            for (let i = 0; i < currentMatches.length; i++)
                currentMatches[i].color = colors[i];

            this.setState({ allMatches: currentMatches, userInfo: userInfo });

            if (!checkpoint.completed) {
                setTimeout(await this.getCheckpointUpdate(checkpointId), 2000);
            }

        } else if (checkpointResponse.status == 400) {

            const props = {
                "error": "BadRequest",
                "usernameAndTagline": ""
            };

            this.state.changePage("Home", props, "UserProfile");
        } else {
            //Show Network Error
            const props = {
                "error": "Server"
            };

            this.state.changePage("Home", props, "UserProfile");
        }

    }

    /** Fetches a list of skills from the backend using the selected matches. */
    async fetchSkills() {
        let username = this.state.userInfo.username;
        let rank = RankedHelper.GetRankedName(this.state.userInfo.rankSoloDuo.rank);
        let tier = RankedHelper.GetRankedTierName(this.state.userInfo.rankSoloDuo.tier);
        let matches = this.state.selectedMatches.map(a => a.id);

        this.setState({ skills: [] });

        if (matches.length == 0) {
            return;
        }

        let json = {
            "username": username,
            "rank": rank,
            "division": tier,
            "matchIDs": matches
        };

        let strJson = JSON.stringify(json);

        const response = await fetch("api/skills/get/", {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: strJson
        });

        await new Promise(r => setTimeout(r, 1000));

        if (response.status == 200) {

            const skills = await response.json();
            this.setState({ skills: skills });

        } else if (response.status == 400) {

            console.log("Error with skills! (400)");

        } else {
            console.log("Error with skills! (Misc)");
        }

    }

    /**
     * Generates an empty user info object. This populates the user info while it is loading.
     * @param {any} usernameAndTagline The username and tagline to use in the empty user info.
     */
    GetDefaultUserInfo(usernameAndTagline) {
        let split = usernameAndTagline.split("#");

        if (split.length != 2) {
            split = ["", ""]
        }

        return {
            "username": split[0],
            "tagline": split[1],
            "rankFlex": {
                "rank": -2,
                "tier": 0,
                "lp": 0,
            },
            "rankSoloDuo": {
                "rank": -2,
                "tier": 0,
                "lp": 0,
            }
        };
    }

    /** Testing function that generates an array of fake matches */
    GetFakeMatches() {
        return [
            {
                "id": "NA_1",
                "queue": 0,
                "champion": "Zeri",
                "position": "MID",
                "averageRank": 6,
                "won": true,
                "playedOn": 1647304576,
                "gold": 19500,
                "kills": 7,
                "deaths": 12,
                "assists": 6
            },
            {
                "id": "NA_2",
                "queue": 1,
                "champion": "DrMundo",
                "position": "MID",
                "averageRank": 0,
                "won": false,
                "playedOn": 1647290191,
                "gold": 19500,
                "kills": 12,
                "deaths": 7,
                "assists": 3
            },
            {
                "id": "NA_3",
                "queue": 0,
                "champion": "MonkeyKing",
                "position": "BOT",
                "averageRank": 7,
                "won": false,
                "playedOn": 1647290191,
                "gold": 19500,
                "kills": 9,
                "deaths": 2,
                "assists": 4
            },
            {
                "id": "NA_4",
                "queue": 0,
                "champion": "Zeri",
                "position": "MID",
                "averageRank": 6,
                "won": true,
                "playedOn": 1647290191,
                "gold": 19500,
                "kills": 7,
                "deaths": 12,
                "assists": 6
            },
            {
                "id": "NA_5",
                "queue": 1,
                "champion": "DrMundo",
                "position": "MID",
                "averageRank": 0,
                "won": false,
                "playedOn": 21847214,
                "gold": 19500,
                "kills": 12,
                "deaths": 7,
                "assists": 3
            },
            {
                "id": "NA_6",
                "queue": 0,
                "champion": "MonkeyKing",
                "position": "BOT",
                "averageRank": 7,
                "won": false,
                "playedOn": 312832111,
                "gold": 19500,
                "kills": 9,
                "deaths": 2,
                "assists": 4
            },
            {
                "id": "NA_7",
                "queue": 0,
                "champion": "Zeri",
                "position": "MID",
                "averageRank": 6,
                "won": true,
                "playedOn": 192883851,
                "gold": 19500,
                "kills": 7,
                "deaths": 12,
                "assists": 6
            },
            {
                "id": "NA_8",
                "queue": 1,
                "champion": "DrMundo",
                "position": "MID",
                "averageRank": 0,
                "won": false,
                "playedOn": 21847214,
                "gold": 19500,
                "kills": 12,
                "deaths": 7,
                "assists": 3
            },
            {
                "id": "NA_9",
                "queue": 0,
                "champion": "MonkeyKing",
                "position": "BOT",
                "averageRank": 7,
                "won": false,
                "playedOn": 312832111,
                "gold": 19500,
                "kills": 9,
                "deaths": 2,
                "assists": 4
            },
            {
                "id": "NA_10",
                "queue": 0,
                "champion": "Zeri",
                "position": "MID",
                "averageRank": 6,
                "won": true,
                "playedOn": 192883851,
                "gold": 19500,
                "kills": 7,
                "deaths": 12,
                "assists": 6
            },
            {
                "id": "NA_11",
                "queue": 1,
                "champion": "DrMundo",
                "position": "MID",
                "averageRank": 0,
                "won": false,
                "playedOn": 21847214,
                "gold": 19500,
                "kills": 12,
                "deaths": 7,
                "assists": 3
            },
            {
                "id": "NA_12",
                "queue": 0,
                "champion": "MonkeyKing",
                "position": "BOT",
                "averageRank": 7,
                "won": false,
                "playedOn": 312832111,
                "gold": 19500,
                "kills": 9,
                "deaths": 2,
                "assists": 4
            }
        ];
    }

    /** Testing functions that generates an array of fake skills. */
    GetFakeAnalysis() {
        let allSkills = [
            { name: "Poor CS", good: false },
            { name: "Low Vision Score", good: false },
            { name: "High Kill Participation", good: true },
            { name: "Low Deaths", good: true },
            { name: "Low Healing", good: false },
            { name: "High Kills", good: true },
            { name: "Low Gold", good: false },
            { name: "Low Damage", good: false },
        ];

        let skills = [];

        for (let i = 0; i < Math.random() * allSkills.length; i++) {
            skills.push(allSkills[i]);
        }
        return skills;
    }

}