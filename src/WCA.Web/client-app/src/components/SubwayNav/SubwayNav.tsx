import { styled } from 'office-ui-fabric-react';
import { SubwayNavBase } from './SubwayNavBase';
import { getSubwayNavStyles } from './SubwayNav.styles';
import { ISubwayNavProps, ISubwayNavStyles, ISubwayNavStyleProps } from './SubwayNav.types';

export const SubwayNav: React.StatelessComponent<ISubwayNavProps> = styled<ISubwayNavProps, ISubwayNavStyleProps, ISubwayNavStyles>(
    SubwayNavBase,
    getSubwayNavStyles,
    undefined
);
