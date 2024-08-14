import {
  TypographyH2,
  TypographyLead
} from '@/components/typography/typography';

async function getHomePageInfo() {
  const response = await fetch('http://localhost/api/Home');
  const data = await response.json();
  return data.body;
}

export default async function HomePage() {
  return (
    <div className="mx-auto my-auto h-[40%] text-center">
      <TypographyH2>SalesWebDashboard</TypographyH2>
      <TypographyLead>{getHomePageInfo()}</TypographyLead>
    </div>
  );
}
