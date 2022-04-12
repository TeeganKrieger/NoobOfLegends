import React, { Component } from 'react';
import './UserProfile.css';
import UserInfo from './UserInfo'
import MatchList from './MatchList'
import StatSelector from './StatSelector'
import StatChart from './StatChart'
import GetColorSet from '../Helpers/DistinctColorGenerator'
import SkillDisplay from './SkillDisplay';

/* Component that renders an entire user profile using various sub components */
export default class UserProfile extends Component {

    constructor(props) {
        super(props);
        //let matches = props.additionalProps.matches;
        //let user = props.additionalProps.info;
        

        this.state = {
            allMatches: [], userInfo: null, selectedMatches: [], activeStat: { id: "gold", name: "Gold", lambda: (m) => m.gold }, skills: [],
            changePage: props.changePageFunc
        };
    }

    componentDidMount() {
        this.fetchCheckpointId(this.props.additionalProps.searchFor);
    }

    /* An array containing all tracked stats, their ids, names and lambda expressions */
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

    /* A lambda expression passed to StatSelector components to facilitate changing the selected stat */
    selectStatFunc = (stat) => {
        this.setState({ activeStat: stat });
    }

    /* A lambda expression passed to MatchListing components to facilitate selecting a match */
    selectMatch = (match) => {
        let selected = this.state.selectedMatches ?? [];
        this.setState({ selectedMatches: [] }); //Little fix for react error when modifying arrays within state
        selected.push(match);
        this.setState({ selectedMatches: selected });
    }

    /* A lambda expression passed to MatchListing components to facilitate unselecting a match */
    deselectMatch = (match) => {
        let matches = this.state.selectedMatches ?? [];
        let newMatches = matches.filter((m) => m.id != match.id);
        this.setState({ selectedMatches: newMatches });
    }

    /* A function that populates skills by fetching analysis from the backend */
    populateSkills() {
        console.log("Repopulating Skills");
        this.setState({ skills: this.GetFakeAnalysis() });
    }

    render() {

        var statSelectors = [];

        for (let i = 0; i < this.allStats.length; i++) {
            statSelectors.push(<StatSelector key={i} stat={this.allStats[i]} selectStatFunc={this.selectStatFunc} />)
        }

        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-12 col-md-7 col-xl-5'>
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
                        <button className="analyze" onClick={this.populateSkills.bind(this)}>Analyze</button>
                    </div>
                </div>
            </div>
        );
    }

    //Fetch
    /* Initiates a profile task on the backend and gets a checkpoint id. */
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

    /* Uses a checkpoint id to recieve occasional updates from the backend to populate the page. */
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

            let colors = GetColorSet(currentMatches.length);

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

    //Testing
    GetFakeUserInfo() {
        return {
            "username": "Saltymate8",
            "tagline": "NA1",
            "rankFlex": {
                "rank": 7,
                "tier": 0,
                "lp": 45,
            },
            "rankSoloDuo": {
                "rank": 5,
                "tier": 3,
                "lp": 98,
            }
        };
    }

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