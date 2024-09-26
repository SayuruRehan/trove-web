import React from 'react';
import styled, { keyframes } from 'styled-components';

// Keyframes for the animations
const plBefore = keyframes`
    from {
        transform: rotate(0) translate(-100%,-100%) rotate(-90deg);
    }
    20% {
        transform: rotate(0) translate(-100%,-100%) rotate(90deg);
    }
    40% {
        transform: rotate(0) translate(-300%,-100%) rotate(90deg);
    }
    60% {
        transform: rotate(90deg) translate(-300%,-100%) rotate(-90deg);
    }
    80% {
        transform: rotate(90deg) translate(-100%,-100%) rotate(-90deg);
    }
    to {
        transform: rotate(90deg) translate(-100%,100%) rotate(90deg);
    }
`;

const plAfter = keyframes`
    from,
    20% {
        transform: rotate(0) scale(1,1);
    }
    40% {
        transform: rotate(0) scale(3,1);
    }
    60% {
        transform: rotate(90deg) scale(3,1);
    }
    80% {
        transform: rotate(90deg) scale(1,1);
    }
    to {
        transform: rotate(90deg) scale(1,1) translate(0,200%);
    }
`;

// Styles for the loading spinner
const Container = styled.div`
    --hue: 223;
    //--bg: hsl(var(--hue), 10%, 90%);
    --fg: hsl(var(--hue), 10%, 10%);
    --trans-dur: 0.3s;
    --trans-timing: cubic-bezier(0.65, 0, 0.35, 1);
    --size: 8em;

    background-color: var(--bg);
    color: var(--fg);
    margin: auto;
    position: relative;
    width: var(--size);
    height: var(--size);

    &:before,
    &:after {
        animation: ${plBefore} 2.5s var(--trans-timing) infinite;
        background-color: currentColor;
        content: "";
        display: block;
        position: absolute;
        top: 75%;
        left: 50%;
        width: 25%;
        height: 25%;
        transform-origin: 100% 100%;
    }

    &:after {
        animation: ${plAfter} 2.5s var(--trans-timing) infinite;
    }

    /* Dark theme */
    @media (prefers-color-scheme: dark) {
        --bg: hsl(var(--hue), 10%, 10%);
        --fg: hsl(var(--hue), 10%, 90%);
    }
`;

// LoadingSpinner Component
const LoadingSpinner = () => {
    return <Container />;
};

export default LoadingSpinner;
