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
            let url = skills[i].url;
            if (url == "")
                url = null;

            let anchor = <a href={url} className='skill-anchor' target="_blank">{skills[i].skillName}</a>;
            if (url == null) {
                anchor = <span className='skill-span'>{skills[i].skillName}</span>;
            }

            skillsArr.push(
                <div key={"skill_" + i} className={"skill " + (skills[i].good ? "good" : "bad")}>
                    {anchor}
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