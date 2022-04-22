import React, { Component } from 'react';
import './SkillDisplay.css';

/* Component that displays multiple skills */
export default class SkillDisplay extends Component {

    constructor(props) {
        super(props);
        this.state = {
            skills: props.skills
        };
    }

    componentWillReceiveProps(props) {
        this.state = { skills: props.skills };
    }

    render() {

        let skills = this.state.skills;

        let skillsArr = [];

        for (let i = 0; i < skills.length; i++) {
            skillsArr.push(
                <div key={"skill_" + i} className={"skill " + (skills[i].good ? "good" : "bad")}>
                    <a src={skills[i].url}>{skills[i].skillName}</a>
                </div>
            );
        }

        return (
            <div className="row">
                {skillsArr}
            </div>
        );
    }
}