import { PropsWithChildren } from 'react';

export function TypographyH2(props: PropsWithChildren) {
  return (
    <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight first:mt-0">
      {props.children}
    </h2>
  );
}

export function TypographyLead(props: PropsWithChildren) {
  return <p className="text-xl text-muted-foreground">{props.children}</p>;
}
